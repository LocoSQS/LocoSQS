using System.Xml.Serialization;

namespace LocoSQS.Model.ActionResults.Properties;

public class ListQueueTagsResult
{
    [XmlElement("Tag")] 
    public List<Tag> Tags { get; set; }

    public ListQueueTagsResult()
    {
    }

    public ListQueueTagsResult(List<Tag> tags)
    {
        Tags = tags;
    }
    
    public ListQueueTagsResult(Dictionary<string, string> tags) : this(tags.Select(x => new Tag(x.Key, x.Value)).ToList())
    {
    }
}