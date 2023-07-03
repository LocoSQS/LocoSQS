using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using Newtonsoft.Json;

namespace LocoSQS.Model.QueueJsonActions;

public class SetQueueAttributes : IQueueJsonAction
{
    [JsonProperty(Required = Required.Always)]
    public Dictionary<string, string> Attributes { get; set; } = new();

    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.SetQueueAttributes(Attributes);
}