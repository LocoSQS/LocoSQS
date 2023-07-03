using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.QueueActions;

public class SetQueueAttributes : IQueueAction
{
    [DataProperty("Attribute")]
    [AsList]
    public List<QueueAttribute> Attributes { get; set; } = new();

    public ActionResult Run(IQueueHandler handler, IQueue queue)
    {
        Dictionary<string, string> attributes = new();
        Attributes.ForEach(x => attributes.Add(x.Name, x.Value));
        return queue.SetQueueAttributes(attributes);
    }
}