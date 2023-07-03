using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.QueueActions;

public class SendMessage : IQueueAction
{
    [DataProperty(false)]
    [AsInt(0, 900)]
    public int? DelaySeconds { get; set; }
    
    [DataProperty(false)]
    [AsList(10)]
    public List<MessageUserAttributeIn> MessageAttribute { get; set; } = new();
    
    [DataProperty]
    [AsString(1, int.MaxValue)]
    public string MessageBody { get; set; }
    
    [DataProperty(false)]
    [AsString]
    public string MessageDeduplicationId { get; set; }
    
    [DataProperty(false)]
    [AsString]
    public string MessageGroupId { get; set; }

    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.SendMessage(DelaySeconds, MessageAttribute.Select(x => x.ToMessageUserAttribute()).ToList(),
            MessageBody, MessageDeduplicationId, MessageGroupId);
}