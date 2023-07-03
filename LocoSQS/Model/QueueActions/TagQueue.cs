using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.QueueActions;

public class TagQueue : IQueueAction
{
    [DataProperty("Tag")]
    [AsList]
    public List<Tag> Tags { get; set; } = new();

    public ActionResult Run(IQueueHandler handler, IQueue queue)
    {
        Dictionary<string, string> tags = new();
        Tags.ForEach(x => tags[x.Key] = x.Value);
        return queue.TagQueue(tags);
    }
}