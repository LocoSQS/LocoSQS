using LocoSQS.Model.Utils;

namespace LocoSQS.Model.Interfaces;

public interface IRootJsonAction
{
    public ActionResult Run(IQueueHandler handler); 
}