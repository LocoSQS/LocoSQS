using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.QueueActions
{
    public class DeleteMessageBatch : IQueueAction
    {
        [DataProperty("DeleteMessageBatchRequestEntry")]
        [AsList]
        public List<DeleteMessageBatchRequestEntry> Entries { get; set; } = new();

        public ActionResult Run(IQueueHandler handler, IQueue queue)
            => queue.DeleteMessageBatch(Entries);
    }
}
