using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.Utils
{
    public class DeleteMessageBatchRequestEntry
    {
        [DataProperty]
        [AsString]
        public string Id { get; set; }
        [DataProperty]
        [AsString]
        public string ReceiptHandle { get; set; }
    }
}
