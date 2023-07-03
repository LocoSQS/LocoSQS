using LocoSQS.Exeptions;
using LocoSQS.Extensions;
using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Storage;
using LocoSQS.Model.Utils;

namespace LocoSQS.Queue;

public class Message : IMessage
{
    public string Id { get; } = Guid.NewGuid().ToString();
    public string Body { get; }
    public string Receipt { get; private set; } = Guid.NewGuid().ToString();
    public DateTimeOffset AvailableFrom { get; private set; } = DateTimeOffset.Now;
    public bool IsAvailable => AvailableFrom <= DateTimeOffset.Now;
    public DateTimeOffset? FirstReceived { get; private set; } = null;
    public int ReceiveCount { get; private set; } = 0;
    public DateTimeOffset Created { get; private set; } = DateTimeOffset.Now;
    public List<MessageUserAttribute> UserAttributes { get; private set; } = new();
    public string? DeadLetterQueueSourceArn { get; set; }

    public event Action<DateTimeOffset, IMessage>? OnMessageReady;
    public event Action<IMessage>? OnMessageInvisible;
    public event Action<IMessage>? OnMessageDeleted;
    
    public Message(string body, TimeSpan timeout)
    {
        if (timeout > TimeSpan.Zero)
            AvailableFrom = DateTimeOffset.Now + timeout;
        
        Body = body;
    }
    
    public Message(string body, TimeSpan timeout, List<MessageUserAttribute> userAttributes) : this(body, timeout)
    {
        UserAttributes = userAttributes;
    }

    public Message(string id, string body, string receipt, TimeSpan timeout, List<MessageUserAttribute> userAttributes, Dictionary<string, string> attributes) : this(body, timeout, userAttributes)
    {
        Id = id;
        Receipt = receipt;

        void QueueConstructorHelper(string key, Action<string> set)
        {
            if (attributes.ContainsKey(key))
                set(attributes[key]);
        }

        QueueConstructorHelper("ApproximateFirstReceiveTimestamp", x => FirstReceived = DateTimeOffset.FromUnixTimeSeconds(long.Parse(x)));
        QueueConstructorHelper("ApproximateReceiveCount", x => ReceiveCount = int.Parse(x));
        QueueConstructorHelper("SentTimestamp", x => Created = DateTimeOffset.FromUnixTimeSeconds(long.Parse(x)));
        QueueConstructorHelper("DeadLetterQueueSourceArn", x => DeadLetterQueueSourceArn = x);
    }

    public void UpdateVisibilityTimeout(TimeSpan timeout)
    {
        if (IsAvailable)
            throw new SQSException("The specified message isn't in flight.", 400, "AWS.SimpleQueueService.MessageNotInflight");

        AvailableFrom = DateTimeOffset.Now + timeout;

        if (timeout == TimeSpan.Zero)
            OnMessageReady?.Invoke(DateTimeOffset.Now, this);
    }

    public Message Recieve(TimeSpan timeout, bool silent)
    {
        OnMessageReady?.Invoke(AvailableFrom, this);
        
        if (timeout > TimeSpan.Zero)
            AvailableFrom = DateTimeOffset.Now + timeout;
        
        if (!silent)
        {
            Receipt = Guid.NewGuid().ToString();
            FirstReceived ??= DateTimeOffset.Now;
            ReceiveCount++;
        }
        
        OnMessageInvisible?.Invoke(this);
        return this;
    }

    public MessageResponse ToResponse(List<string>? requestedAttributes = null, List<string>? requestedUserAttributes = null)
    {
        requestedAttributes ??= new();
        requestedUserAttributes ??= new();

        Dictionary<string, string> attributes = ConstructAttributes(requestedAttributes);

        List<QueueAttribute> convertedAttributes = new();
        foreach (var attribute in attributes)
            convertedAttributes.Add(new()
            {
                Name = attribute.Key,
                Value = attribute.Value
            });


        bool allUserAttributes = requestedUserAttributes.Contains("All") || requestedUserAttributes.Contains(".*");
        List<MessageUserAttribute> userAttributes = UserAttributes
            .Where(x => allUserAttributes || requestedUserAttributes.Any(y =>
                (y.Contains(".*"))
                    ? x.Name.StartsWith(y.Replace(".*", ""))
                    : x.Name == y)).ToList();
        return new(Id, Receipt, Body, convertedAttributes, userAttributes);
    }

    public Dictionary<string, string> ConstructAttributes(List<string> requested)
    {
        Dictionary<string, string> items = new();

        void AttributeHelper(string name, Func<string> value)
        {
            if (requested.Contains("All") || requested.Contains(name))
                items.Add(name, value());
        }

        if (FirstReceived != null)
            AttributeHelper("ApproximateFirstReceiveTimestamp", () => FirstReceived!.Value.ToUnixTimeSeconds().ToString());
        AttributeHelper("ApproximateReceiveCount", ReceiveCount.ToString);
        AttributeHelper("SenderId", () => "ABCDEFGHI1JKLMNOPQ23R");
        AttributeHelper("SentTimestamp", Created.ToUnixTimeSeconds().ToString);
        if (DeadLetterQueueSourceArn != null)
            AttributeHelper("DeadLetterQueueSourceArn", () => DeadLetterQueueSourceArn);

        return items;
    }

    public void Delete()
    {
        OnMessageDeleted?.Invoke(this);
    }
}