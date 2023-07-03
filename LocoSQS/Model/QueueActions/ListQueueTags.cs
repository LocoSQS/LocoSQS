using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;

namespace LocoSQS.Model.QueueActions;

public class ListQueueTags : IQueueAction, IQueueJsonAction
{
    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.ListQueueTags();
}