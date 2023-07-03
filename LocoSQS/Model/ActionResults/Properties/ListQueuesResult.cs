using System.Xml.Serialization;

namespace LocoSQS.Model.ActionResults.Properties;

public class ListQueuesResult
{
    [XmlElement("QueueUrl")]
    public List<string> QueueUrls { get; set; }

    [XmlElement("NextToken")] 
    public string? NextToken { get; set; } = null;

    public ListQueuesResult(List<string> queueUrls, string? nextToken)
    {
        QueueUrls = queueUrls;
        NextToken = nextToken;
    }

    public ListQueuesResult()
    {
    }
}