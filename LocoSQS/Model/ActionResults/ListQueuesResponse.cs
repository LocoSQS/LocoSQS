using System.Xml.Serialization;
using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.ActionResults;

[XmlRoot(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
public class ListQueuesResponse : IActionResultData
{
    public ListQueuesResult ListQueuesResult { get; set; }
    
    public ResponseMetadata ResponseMetadata { get; set; } = new();

    public ListQueuesResponse(List<string> queueUrls, string? nextToken)
    {
        ListQueuesResult = new(queueUrls, nextToken);
    }

    public ListQueuesResponse()
    {
    }

    public object JsonResult => ListQueuesResult;
}