using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Test.Parser.Data;

public class Data2 : IQueueAction
{
    [DataProperty]
    [AsList]
    public List<string> Data { get; set; }

    public ActionResult Run(IQueueHandler handler, IQueue queue)
    {
        throw new NotImplementedException();
    }
}