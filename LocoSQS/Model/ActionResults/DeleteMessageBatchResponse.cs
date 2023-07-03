using LocoSQS.Model.ActionResults.Properties;
using System.Xml.Serialization;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.ActionResults
{
    [XmlRoot(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
    public class DeleteMessageBatchResponse : IActionResultData
    {
        public DeleteMessageBatchResult DeleteMessageBatchResult { get; set; }
        public ResponseMetadata ResponseMetadata { get; set; } = new();

        public DeleteMessageBatchResponse(DeleteMessageBatchResult deleteMessageBatchResult)
        {
            DeleteMessageBatchResult = deleteMessageBatchResult;
        }

        public DeleteMessageBatchResponse()
        {
        }

        public DeleteMessageBatchResponse(List<DeleteMessageBatchResultEntry> entries, List<BatchResultErrorEntry> errors) : this(new(entries, errors))
        {
        }

        public DeleteMessageBatchResponse(List<string> entries, List<BatchResultErrorEntry> errors) : this(entries.Select(x => new DeleteMessageBatchResultEntry(x)).ToList(), errors)
        {
        }

        public object JsonResult => DeleteMessageBatchResult;
    }
}
