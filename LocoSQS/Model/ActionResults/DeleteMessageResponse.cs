using LocoSQS.Model.ActionResults.Properties;
using System.Xml.Serialization;
using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;

namespace LocoSQS.Model.ActionResults
{
    [XmlRoot(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
    public class DeleteMessageResponse : IActionResultData
    {
        public ResponseMetadata ResponseMetadata { get; set; } = new();
        public object JsonResult => Empty.Instance;
    }
}
