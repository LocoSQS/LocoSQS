using System.Xml.Serialization;
using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.ActionResults;

[XmlRoot(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
public class GetQueueUrlResponse : IActionResultData
{
    public GetQueueUrlResult GetQueueUrlResult { get; set; }
    public ResponseMetadata ResponseMetadata { get; set; } = new();

    public GetQueueUrlResponse()
    {
    }

    public GetQueueUrlResponse(GetQueueUrlResult getQueueUrlResult)
    {
        GetQueueUrlResult = getQueueUrlResult;
    }

    public GetQueueUrlResponse(string qUrl) : this(new GetQueueUrlResult(){ QueueUrl = qUrl })
    {
    }

    public object JsonResult => GetQueueUrlResult;
}