using LocoSQS.Model.Utils;

namespace LocoSQS.Model.Interfaces;

public interface IQueueJsonAction
{
    public ActionResult Run(IQueueHandler handler, IQueue queue);
}