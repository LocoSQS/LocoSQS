using System.Xml.Serialization;
using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Interfaces;
using LocoSQS.Queue;

namespace LocoSQS.Model.ActionResults;

[XmlRoot(Namespace = "http://queue.amazonaws.com/doc/2012-11-05/", IsNullable = true)]
public class ReceiveMessageResponse : IActionResultData
{
    [XmlArrayItem(typeof(MessageResponse), ElementName = "Message")]
    public List<MessageResponse> ReceiveMessageResult { get; set; }

    public ResponseMetadata ResponseMetadata { get; set; } = new();

    public ReceiveMessageResponse(List<MessageResponse> receiveMessageResult)
    {
        ReceiveMessageResult = receiveMessageResult;
    }

    public ReceiveMessageResponse() { }
    public object JsonResult => new ReceiveMessageResponseJson(ReceiveMessageResult);
}

public class ReceiveMessageResponseJson
{
    public List<MessageResponseJson> Messages { get; set; }

    public ReceiveMessageResponseJson()
    {
    }

    public ReceiveMessageResponseJson(List<MessageResponse> messages)
    {
        Messages = messages.Select(x => new MessageResponseJson(x)).ToList();
    }
}