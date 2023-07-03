using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.QueueActions
{
    public class ChangeMessageVisibilityBatch : IQueueAction
    {
        [DataProperty("ChangeMessageVisibilityBatchRequestEntry")]
        [AsList]
        public List<ChangeMessageVisibilityBatchRequestEntry> ChangeMessageVisibilityBatchRequestEntries { get; set; }

        public ActionResult Run(IQueueHandler handler, IQueue queue)
            => queue.ChangeMessageVisibilityBatch(ChangeMessageVisibilityBatchRequestEntries);
    }
}
