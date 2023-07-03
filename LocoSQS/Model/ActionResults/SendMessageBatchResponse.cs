using LocoSQS.Model.ActionResults.Properties;
using System.Xml.Serialization;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.ActionResults
{
    [XmlRoot(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
    public class SendMessageBatchResponse : IActionResultData
    {
        public SendMessageBatchResult SendMessageBatchResult { get; set; }
        public ResponseMetadata ResponseMetadata { get; set; } = new();

        public SendMessageBatchResponse()
        {
        }

        public SendMessageBatchResponse(SendMessageBatchResult sendMessageBatchResult)
        {
            SendMessageBatchResult = sendMessageBatchResult;
        }

        public SendMessageBatchResponse(List<SendMessageBatchResultEntry> entries, List<BatchResultErrorEntry> errors) : this(new(entries, errors)) { }
        public object JsonResult => SendMessageBatchResult;
    }
}
