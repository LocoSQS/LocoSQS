using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;
using Newtonsoft.Json;

namespace LocoSQS.Model.QueueActions;

public class DeleteMessage : IQueueAction, IQueueJsonAction
{
    [DataProperty]
    [AsString]
    [JsonProperty(Required = Required.Always)]
    public string ReceiptHandle { get; set; }

    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.DeleteMessage(ReceiptHandle);
}