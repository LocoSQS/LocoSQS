using LocoSQS.Model.ActionResults.Properties;
using System.Xml.Serialization;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.ActionResults
{
    [XmlRootAttribute(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
    public class CreateQueueResponse : IActionResultData
    {
        public CreateQueueResult CreateQueueResult { get; set; }
        public ResponseMetadata ResponseMetadata { get; set; }

        public CreateQueueResponse(string qUrl)
        {
            CreateQueueResult = new CreateQueueResult()
            {
                QueueUrl = qUrl
            };

            ResponseMetadata = new ResponseMetadata();
        }

        public CreateQueueResponse()
        {
        }

        public object JsonResult => CreateQueueResult;
    }
}
