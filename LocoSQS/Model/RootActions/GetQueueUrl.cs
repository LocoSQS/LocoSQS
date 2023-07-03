using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;
using Newtonsoft.Json;

namespace LocoSQS.Model.RootActions;

public class GetQueueUrl : IRootAction, IRootJsonAction
{
    [DataProperty]
    [AsString(80)]
    [JsonProperty(Required = Required.Always)]
    public string QueueName { get; set; }
    
    public ActionResult Run(IQueueHandler handler)
        => handler.GetQueueUrl(QueueName, null);
}