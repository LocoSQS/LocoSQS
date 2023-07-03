using LocoSQS.Model.ActionResults.Properties;
using System.Xml.Serialization;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.ActionResults
{
    [XmlRootAttribute(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
    public class ErrorResponse : IActionResultData
    {
        public Error Error { get; set; }
        public string RequestId { get; set; } = Guid.NewGuid().ToString();

        public ErrorResponse(Error error)
        {
            Error = error;
        }

        public ErrorResponse() { }
        public object JsonResult => Error;
    }
}
