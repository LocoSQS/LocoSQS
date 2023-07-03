using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using Newtonsoft.Json;

namespace LocoSQS.Model.QueueJsonActions;

public class SendMessage : IQueueJsonAction
{
    public string? Id { get; set; }
    public int? DelaySeconds { get; set; } = null;
    public Dictionary<string, MessageUserAttributeJson> MessageAttributes { get; set; } = new();

    [JsonProperty(Required = Required.Always)]
    public string MessageBody { get; set; } = "";

    public string? MessageDeduplicationId { get; set; } = null;
    public string? MessageGroupId { get; set; } = null;

    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.SendMessage(DelaySeconds, MessageAttributes.Select(x => new MessageUserAttribute()
        {
            BinaryValue = x.Value.BinaryValue,
            StringValue = x.Value.StringValue,
            DataType = x.Value.DataType,
            Name = x.Key
        }).ToList(), MessageBody, MessageDeduplicationId, MessageGroupId);
}