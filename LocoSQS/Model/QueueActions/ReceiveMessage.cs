using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.QueueActions;

public class ReceiveMessage : IQueueAction, IQueueJsonAction
{
    [DataProperty("AttributeName", false)]
    [AsList]
    public List<string> AttributeNames { get; set; } = new();
    
    [DataProperty(false)]
    [AsInt(1, 10)]
    public int? MaxNumberOfMessages { get; set; }
    
    [DataProperty("MessageAttributeName", false)]
    [AsList]
    public List<string> MessageAttributeNames { get; set; } = new();
    
    [DataProperty(false)]
    [AsString]
    public string? ReceiveRequestAttemptId { get; set; }
    
    [DataProperty(false)]
    [AsInt(0, 43200)]
    public int? VisibilityTimeout { get; set; }
    
    [DataProperty(false)]
    [AsInt(0, 20)]
    public int? WaitTimeSeconds { get; set; }

    /// <summary>
    /// LocoSQS-only Attribute
    /// </summary>
    [DataProperty(false)]
    [AsInt(0, 1)]
    public int? Silent { get; set; }

    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.ReceiveMessage(AttributeNames, MaxNumberOfMessages, MessageAttributeNames, ReceiveRequestAttemptId,
            VisibilityTimeout, WaitTimeSeconds, (Silent.HasValue) ? Silent.Value == 1 : null);
}