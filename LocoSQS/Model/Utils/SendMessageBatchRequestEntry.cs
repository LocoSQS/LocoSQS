using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.Utils
{
    public class SendMessageBatchRequestEntry
    {
        [DataProperty]
        [AsString]
        public string Id { get; set; }
        [DataProperty(false)]
        [AsInt(0, 900)]
        public int? DelaySeconds { get; set; }
        [DataProperty(false)]
        [AsList]
        public List<MessageUserAttributeIn> Attributes { get; set; } = new();
        [DataProperty]
        [AsString]
        public string MessageBody { get; set; }
        [DataProperty(false)]
        [AsString]
        public string? MessageDeduplicationId { get; set; }
        [DataProperty(false)]
        [AsString]
        public string? MessageGroupId { get; set; }
    }
}
