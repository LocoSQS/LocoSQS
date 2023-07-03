using System.Xml.Serialization;
using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.ActionResults;

[XmlRoot(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
public class ListQueueTagsResponse : IActionResultData
{
    public ListQueueTagsResult ListQueueTagsResult { get; set; }
    public ResponseMetadata ResponseMetadata { get; set; }
    
    public ListQueueTagsResponse()
    {
    }

    public ListQueueTagsResponse(ListQueueTagsResult listQueueTagsResult)
    {
        ListQueueTagsResult = listQueueTagsResult;
    }
    
    public ListQueueTagsResponse(Dictionary<string,string> listQueueTagsResult) : this(new ListQueueTagsResult(listQueueTagsResult))
    {
    }

    public object JsonResult => new ListQueueTagsResponseJson(ListQueueTagsResult);
}

public class ListQueueTagsResponseJson
{
    public Dictionary<string, string> Tags { get; set; } = new();
    
    public ListQueueTagsResponseJson(){}

    public ListQueueTagsResponseJson(ListQueueTagsResult result)
    {
        result.Tags.ForEach(x => Tags.Add(x.Key, x.Value));
    }
}