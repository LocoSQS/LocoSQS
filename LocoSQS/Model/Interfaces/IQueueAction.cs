using LocoSQS.Model.Utils;

namespace LocoSQS.Model.Interfaces;

public interface IQueueAction
{
    public ActionResult Run(IQueueHandler handler, IQueue queue);
}