using System.Xml.Serialization;
using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.ActionResults;

[XmlRoot(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
public class ListDeadLetterSourceQueuesResponse : IActionResultData
{
    public ListDeadLetterSourceQueuesResult ListDeadLetterSourceQueuesResult { get; set; }
    public ResponseMetadata ResponseMetadata { get; set; } = new();

    public ListDeadLetterSourceQueuesResponse()
    {
    }

    public ListDeadLetterSourceQueuesResponse(ListDeadLetterSourceQueuesResult listDeadLetterSourceQueuesResult)
    {
        ListDeadLetterSourceQueuesResult = listDeadLetterSourceQueuesResult;
    }
    
    public ListDeadLetterSourceQueuesResponse(List<string> queueUrls, string? nextToken) : this(new ListDeadLetterSourceQueuesResult(queueUrls, nextToken))
    {
    }

    public object JsonResult => ListDeadLetterSourceQueuesResult;
}