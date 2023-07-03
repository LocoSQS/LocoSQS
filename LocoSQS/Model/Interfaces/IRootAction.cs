using LocoSQS.Model.Utils;

namespace LocoSQS.Model.Interfaces;

public interface IRootAction
{
    public ActionResult Run(IQueueHandler handler); 
}