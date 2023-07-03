using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.Utils
{
    public class ChangeMessageVisibilityBatchRequestEntry
    {
        [DataProperty]
        [AsString]
        public string Id { get; set; }
        [DataProperty]
        [AsString]
        public string ReceiptHandle { get; set; }
        [DataProperty]
        [AsInt(0, 43200)]
        public int VisibilityTimeout { get; set; }
    }
}
