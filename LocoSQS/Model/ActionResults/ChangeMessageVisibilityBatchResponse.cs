using LocoSQS.Model.ActionResults.Properties;
using System.Xml.Serialization;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.ActionResults
{
    [XmlRoot(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
    public class ChangeMessageVisibilityBatchResponse : IActionResultData
    {
        public ChangeMessageVisibilityBatchResult ChangeMessageVisibilityBatchResult { get; set; }
        public ResponseMetadata ResponseMetadata { get; set; } = new();

        public ChangeMessageVisibilityBatchResponse()
        {
        }

        public ChangeMessageVisibilityBatchResponse(ChangeMessageVisibilityBatchResult changeMessageVisibilityBatchResult)
        {
            ChangeMessageVisibilityBatchResult = changeMessageVisibilityBatchResult;
        }

        public ChangeMessageVisibilityBatchResponse(List<ChangeMessageVisibilityBatchResultEntry> entries) : this(new ChangeMessageVisibilityBatchResult(entries))
        {
        }

        public ChangeMessageVisibilityBatchResponse(List<ChangeMessageVisibilityBatchResultEntry> entries, List<BatchResultErrorEntry> errorEntries) : this(new ChangeMessageVisibilityBatchResult(entries, errorEntries))
        {
        }

        public ChangeMessageVisibilityBatchResponse(List<string> entries) : this(new ChangeMessageVisibilityBatchResult(entries.Select(x => new ChangeMessageVisibilityBatchResultEntry(x)).ToList()))
        {
        }

        public ChangeMessageVisibilityBatchResponse(List<string> entries, List<BatchResultErrorEntry> errorEntries) : this(new ChangeMessageVisibilityBatchResult(entries.Select(x => new ChangeMessageVisibilityBatchResultEntry(x)).ToList(), errorEntries))
        {
        }

        public object JsonResult => ChangeMessageVisibilityBatchResult;
    }
}
