using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.QueueActions
{
    public class GetQueueAttributes : IQueueAction, IQueueJsonAction
    {
        [DataProperty("AttributeName", false)]
        [AsList]
        public List<string> AttributeNames { get; set; } = new();
        
        public ActionResult Run(IQueueHandler handler, IQueue queue)
            => queue.GetQueueAttributes(AttributeNames);
    }
}
