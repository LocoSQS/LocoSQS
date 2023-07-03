using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Test.Parser.Data;

public class Data6 : IQueueAction
{
    [DataProperty]
    [AsString]
    public string DataString { get; set; }
    
    [DataProperty]
    [AsInt]
    public int DataInt { get; set; }
    
    [DataProperty]
    [AsLong]
    public long DataLong { get; set; }

    public ActionResult Run(IQueueHandler handler, IQueue queue)
    {
        throw new NotImplementedException();
    }
}