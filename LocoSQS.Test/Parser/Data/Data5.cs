using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Test.Parser.Data;

public class Data5 : IQueueAction
{
    [DataProperty]
    [AsLong]
    public long Data { get; set; }

    public ActionResult Run(IQueueHandler handler, IQueue queue)
    {
        throw new NotImplementedException();
    }
}