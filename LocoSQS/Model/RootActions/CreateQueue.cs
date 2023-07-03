using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.RootActions
{
    public class CreateQueue : IRootAction
    {
        [DataProperty]
        [AsString]
        public string QueueName { get; set; }
        [DataProperty("Tag", false)]
        [AsList]
        public List<Tag> Tags { get; set; } = new();
        [DataProperty("Attribute", false)]
        [AsList]
        public List<QueueAttribute> Attributes { get; set; } = new();

        public ActionResult Run(IQueueHandler handler)
        {
            Dictionary<string, string> attributes = new();
            Attributes.ForEach(x => attributes[x.Name] = x.Value);

            Dictionary<string, string> tags = new();
            Tags.ForEach(x => tags[x.Key] = x.Value);

            return handler.CreateQueue(attributes, QueueName, tags);
        }
    }
}
