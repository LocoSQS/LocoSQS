using LocoSQS.Exeptions;
using LocoSQS.Extensions;
using LocoSQS.Handler;
using LocoSQS.Model.Interfaces;
using LocoSQS.Parser;
using LocoSQS.Storage;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using System.Collections.Specialized;
using System.Web;
using LocoSQS;
using LocoSQS.Model.Utils;
using LocoSQS.Storage.Json;
using System.Net.WebSockets;
using System.Text;
using System;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System.Xml.Linq;
using System.IO;

ServerConfiguration.Init();

if (!Directory.Exists("./wwwroot"))
{
    Directory.CreateDirectory("./wwwroot");
}

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddCors();
var app = builder.Build();

app.UseWebSockets();

List<Observer> observers = new List<Observer>();
QueueHandler handler = new();

IQueueStorage storage;

switch (ServerConfiguration.STORAGEBACKEND)
{
    case StorageBackendType.Json:
        storage = new JsonStorage(ServerConfiguration.STORAGEBACKENDARG);
        break;
    case StorageBackendType.Memory:
        storage = new MemoryStorage();
        break;
    default:
        throw new Exception("Unknown storage backend");
}

if (ServerConfiguration.DEBUG)
{
    storage.OnUpdateQueue += x => Console.WriteLine($"[OnUpdateQueue] {x.Name}");
    storage.OnDeleteQueue += x => Console.WriteLine($"[OnDeleteQueue] {x.Name}");
    storage.OnUpdateMessage += (x, y) => Console.WriteLine($"[OnUpdateMessage] {x} -> {y.Body}");
    storage.OnDeleteMessage += (x, y) => Console.WriteLine($"[OnDeleteMessage] {x} -> {y.Body}");
}

if (!ServerConfiguration.NOTRACK)
{
    handler.OnNewQueue += (x) =>
    {
        observers.RemoveAll(y => y.Queue.Name == x.Name);
        observers.Add(new(x));
    };
}


handler.Init(storage, ServerConfiguration.READONLY);

app.Map("/0/{name}", async (string name, HttpContext ctx) =>
{
    Observer? observer = observers.Find(x => x.Queue.Name == name);

    if (observer != null && ctx.WebSockets.IsWebSocketRequest)
    {
        using (var webSocket = await ctx.WebSockets.AcceptWebSocketAsync())
        {
            List<ObserverLog> history = observer.GetLogs().ToList();

            await webSocket.SendAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(history)), WebSocketMessageType.Text, true, CancellationToken.None);

            async void SendMessage(ObserverLog x)
            {
                if (webSocket.State == WebSocketState.Open)
                    await webSocket.SendAsync(Encoding.UTF8.GetBytes(JsonConvert.SerializeObject(new List<ObserverLog>() { x })), WebSocketMessageType.Text, true, CancellationToken.None);
            }

            observer.OnUpdate += SendMessage;

            while (webSocket.State == WebSocketState.Open)
            {
                if (webSocket.State == WebSocketState.CloseReceived)
                    Console.WriteLine("Close recieved");

                var buffer = new byte[4096];
                await Task.Delay(TimeSpan.FromMilliseconds(100)); // Unsure if a websocket needs to wait, but just to be sure
                var x = await webSocket.ReceiveAsync(buffer, CancellationToken.None);

                if (x.MessageType == WebSocketMessageType.Close)
                {
                    await webSocket.CloseOutputAsync(WebSocketCloseStatus.NormalClosure, "", CancellationToken.None);
                }
                else if (x.Count > 0)
                {
                    string text = Encoding.UTF8.GetString(buffer, 0, x.Count).Trim();
                    string response = RPS.Check(text) ?? "The websocket is not supposed to recieve messages";
                    await webSocket.SendAsync(Encoding.UTF8.GetBytes(response), WebSocketMessageType.Text, true, CancellationToken.None);
                }

            }

            observer.OnUpdate -= SendMessage;

            return Results.Empty;
        }
    }
    else
    {
        return Results.BadRequest();
    }
});

app.MapPost("/", (HttpContext ctx) =>
{
    bool jsonMode = ctx.Request.Headers["Content-Type"] == "application/x-amz-json-1.0";
    string body = ctx.ReadBodyAsString().GetAwaiter().GetResult();
    
    LocoSQS.Model.Utils.ActionResult result;

    try
    {
        if (jsonMode)
        {
            string action = ctx.Request.Headers["X-Amz-Target"]!;
            action = action.Replace("AmazonSQS.", "");
            JObject parsedObject = JsonConvert.DeserializeObject<JObject>(body)!;
            
            if (parsedObject.ContainsKey("QueueUrl"))
            {
                Type queueActionType = Parser.GetType<IQueueJsonAction>(action);
                string queueUrl = parsedObject["QueueUrl"]!.ToObject<string>()!;
                IQueue? queue = handler.GetQueue(queueUrl);
                
                if (queue == null)
                    throw new NotAuthorized();

                IQueueJsonAction queueAction;
                
                try
                {
                    queueAction = (IQueueJsonAction) JsonConvert.DeserializeObject(body, queueActionType);
                }
                catch (Exception e)
                {
                    throw new InvalidParameterValue(e.Message);
                }

                result = queueAction!.Run(handler, queue);
            }
            else
            {
                Type rootActionType = Parser.GetType<IRootJsonAction>(action);
                IRootJsonAction rootAction;
                
                try
                {
                    rootAction = (IRootJsonAction) JsonConvert.DeserializeObject(body, rootActionType);
                }
                catch (Exception e)
                {
                    throw new InvalidParameterValue(e.Message);
                }

                result = rootAction!.Run(handler);
            }
        }
        else
        {
            NameValueCollection data = HttpUtility.ParseQueryString(body);

            if (data["QueueUrl"] != null)
            {
                IQueue? queue = handler.GetQueue(data["QueueUrl"]!);

                if (queue == null)
                    throw new NotAuthorized();

                IQueueAction action = Parser.Parse<IQueueAction>(data.ToDictionary());
                result = action.Run(handler, queue);
            }
            else
            {
                IRootAction action = Parser.Parse<IRootAction>(data.ToDictionary());
                result = action.Run(handler);
            }
        }
    }
    catch (SQSException e)
    {
        result = e.AsResult;
    }
    catch (Exception e)
    {
        result = new InternalFailure(e.Message).AsResult;
    }

    if (jsonMode)
        return Results.Text(result.Json, "application/json", statusCode: result.HttpStatusCode);
    else
        return Results.Text(result.Xml, "application/xml", statusCode: result.HttpStatusCode);
});

app.MapPost("/0/{name}", (string name, HttpContext ctx) =>
{
    NameValueCollection col = HttpUtility.ParseQueryString(ctx.ReadBodyAsString().GetAwaiter().GetResult());  
    LocoSQS.Model.Utils.ActionResult result;
    IQueue? queue = handler.GetQueue(name);

    try
    {
        if (queue == null)
            throw new NotAuthorized();
        
        IQueueAction action = Parser.Parse<IQueueAction>(col.ToDictionary());
        result = action.Run(handler, queue);
    }
    catch (SQSException e)
    {
        result = e.AsResult;
    }
    catch (Exception e)
    {
        result = new InternalFailure(e.Message).AsResult;
    }

    return Results.Text(result.Xml, "application/xml", statusCode: result.HttpStatusCode);
});

if (app.Environment.IsDevelopment())
{
    app.MapDelete("/stop", () => Environment.Exit(0));
}

app.UseCors(builder => builder
    .AllowAnyOrigin()
    .AllowAnyHeader()
    .AllowAnyMethod()
);

app.UseStaticFiles();

app.MapGet("/", async ctx => ctx.Response.Redirect("/index.html"));

app.Run($"{ServerConfiguration.PROTOCOL}://*:{ServerConfiguration.PORT}");
