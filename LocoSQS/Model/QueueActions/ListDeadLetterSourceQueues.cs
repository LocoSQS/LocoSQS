using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.QueueActions;

public class ListDeadLetterSourceQueues : IQueueAction, IQueueJsonAction
{    
    [DataProperty(false)]
    [AsInt(1, 1000)]
    public int? MaxResults { get; set; } = null;
    
    [DataProperty(false)]
    [AsString]
    public string NextToken { get; set; } = null;

    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => handler.ListDeadLetterSourceQueues(queue, MaxResults, NextToken);
}