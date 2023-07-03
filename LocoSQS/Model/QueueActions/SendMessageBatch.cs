using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.QueueActions
{
    public class SendMessageBatch : IQueueAction
    {
        [DataProperty("SendMessageBatchRequestEntry")]
        [AsList]
        public List<SendMessageBatchRequestEntry> Entries { get; set; } = new();
        public ActionResult Run(IQueueHandler handler, IQueue queue)
            => queue.SendMessageBatch(Entries);
    }
}
