using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.RootActions;

public class ListQueues : IRootAction, IRootJsonAction
{
    [DataProperty(false)]
    [AsInt(1, 1000)]
    public int? MaxResults { get; set; } = null;
    
    [DataProperty(false)]
    [AsString]
    public string NextToken { get; set; } = null;
    
    [DataProperty(false)]
    [AsString]
    public string QueueNamePrefix { get; set; } = null;
    public ActionResult Run(IQueueHandler handler)
        => handler.ListQueues(MaxResults, NextToken, QueueNamePrefix);
}