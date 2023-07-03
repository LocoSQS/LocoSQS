using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.QueueActions
{
    public class PurgeQueue : IQueueAction, IQueueJsonAction
    {
        /// <summary>
        /// LocoSQS-only Attribute
        /// </summary>
        [DataProperty(false)]
        [AsInt]
        public int IgnoreTime { get; set; } = 0;
        public ActionResult Run(IQueueHandler handler, IQueue queue)
            => queue.PurgeQueue(IgnoreTime != 0);
    }
}
