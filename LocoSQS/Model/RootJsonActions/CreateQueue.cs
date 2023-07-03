using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using Newtonsoft.Json;

namespace LocoSQS.Model.RootJsonActions;

public class CreateQueue : IRootJsonAction
{
    public Dictionary<string, string> Attributes { get; set; } = new();

    [JsonProperty(Required = Required.Always)]
    public string QueueName { get; set; } = "";

    public Dictionary<string, string> Tags { get; set; } = new();

    public ActionResult Run(IQueueHandler handler)
        => handler.CreateQueue(Attributes, QueueName, Tags);
}