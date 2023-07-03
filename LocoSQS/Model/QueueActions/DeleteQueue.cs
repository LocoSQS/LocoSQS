using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;

namespace LocoSQS.Model.QueueActions;

public class DeleteQueue : IQueueAction, IQueueJsonAction
{
    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => handler.DeleteQueue(queue.Name);
}