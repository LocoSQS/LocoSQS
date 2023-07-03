using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;
using Newtonsoft.Json;

namespace LocoSQS.Model.QueueActions;

public class AddPermission : IQueueAction, IQueueJsonAction
{
    [DataProperty("ActionName")]
    [AsList]
    [JsonProperty("Actions", Required = Required.Always)]
    public List<string> ActionNames { get; set; }
    
    [DataProperty("AWSAccountId")]
    [AsList]
    [JsonProperty("AWSAccountIds", Required = Required.Always)]
    public List<string> AwsAccountIds { get; set; }
    
    [DataProperty]
    [AsString(80)]
    [JsonProperty(Required = Required.Always)]
    public string Label { get; set; }
    
    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.AddPermission(Label, new(ActionNames, AwsAccountIds));
}