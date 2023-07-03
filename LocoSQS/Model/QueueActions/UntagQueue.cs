using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;
using Newtonsoft.Json;

namespace LocoSQS.Model.QueueActions;

public class UntagQueue : IQueueAction, IQueueJsonAction
{
    [DataProperty("TagKey")]
    [AsList]
    [JsonProperty("TagKeys", Required = Required.Always)]
    public List<string> Keys { get; set; } = new();

    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.UntagQueue(Keys);
}