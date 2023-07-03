using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.QueueActions;

public class RemovePermission : IQueueAction, IQueueJsonAction
{
    [DataProperty]
    [AsString(80)]
    public string Label { get; set; }

    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.RemovePermission(Label);
}