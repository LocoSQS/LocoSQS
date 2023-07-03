using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using Newtonsoft.Json;

namespace LocoSQS.Model.QueueJsonActions;

public class SendMessageBatch : IQueueJsonAction
{
    [JsonProperty(Required = Required.Always)]
    public List<SendMessage> Entries { get; set; } = new();

    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.SendMessageBatch(Entries.Select(x => new SendMessageBatchRequestEntry()
        {
            Id = x.Id ?? throw new Exception("Missing Id"),
            Attributes = x.MessageAttributes.Select(x => new MessageUserAttributeIn()
            {
                Name = x.Key,
                Value = new()
                {
                    BinaryValue = x.Value.BinaryValue,
                    StringValue = x.Value.StringValue,
                    DataType = x.Value.DataType,
                }
            }).ToList(),
            DelaySeconds = x.DelaySeconds,
            MessageBody = x.MessageBody,
            MessageDeduplicationId = x.MessageDeduplicationId,
            MessageGroupId = x.MessageGroupId
        }).ToList());
}