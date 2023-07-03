using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Utils;
using System.Xml.Serialization;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.ActionResults
{
    [XmlRootAttribute(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
    public class GetQueueAttributesResponse : IActionResultData
    {
        [XmlArray("GetQueueAttributesResult")]
        [XmlArrayItem(typeof(QueueAttribute), ElementName = "Attribute")]
        public List<QueueAttribute> Attributes { get; set; } = new();

        public ResponseMetadata ResponseMetadata { get; set; } = new();

        public GetQueueAttributesResponse(List<QueueAttribute> attributes)
        {
            Attributes = attributes;
        }

        public GetQueueAttributesResponse(Dictionary<string,string> attributes)
        {
            Attributes = new();
            foreach (var x in attributes)
            {
                Attributes.Add(new()
                {
                    Name = x.Key,
                    Value = x.Value
                });
            }
        }

        public GetQueueAttributesResponse() { }
        public object JsonResult => new GetQueueAttributesResponseJson(Attributes);
    }

    public class GetQueueAttributesResponseJson
    {
        public Dictionary<string, string> Attributes { get; set; } = new();

        public GetQueueAttributesResponseJson(){}

        public GetQueueAttributesResponseJson(List<QueueAttribute> attributes)
        {
            attributes.ForEach(x => Attributes.Add(x.Name, x.Value));
        }
    }
}
