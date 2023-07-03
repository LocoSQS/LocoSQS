using System.Xml.Serialization;
using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.ActionResults;

[XmlRoot(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
public class SendMessageResponse : IActionResultData
{
    public SendMessageResult SendMessageResult { get; set; }
    public ResponseMetadata ResponseMetadata { get; set; } = new();

    public SendMessageResponse(IMessage message)
    {
        SendMessageResult = message.ToResponse(requestedUserAttributes: new(){"All"}).ToSendResult();
    }

    public SendMessageResponse() { }
    public object JsonResult => SendMessageResult;
}