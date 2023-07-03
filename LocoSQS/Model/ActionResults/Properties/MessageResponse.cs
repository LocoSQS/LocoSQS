using System.Xml.Serialization;
using LocoSQS.Extensions;
using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using Newtonsoft.Json;

namespace LocoSQS.Model.ActionResults.Properties;

public class MessageResponse : IActionResultData
{
    public string MessageId { get; set; }
    public string ReceiptHandle { get; set; }
    public string MD5OfBody { get; set; }
    public string Body { get; set; }
    
    [XmlElement("Attribute")]
    public List<QueueAttribute> Attributes { get; set; }

    public string? MD5OfMessageAttributes { get; set; } = null;

    [XmlElement("MessageAttribute")]
    public List<MessageUserAttributeOut>? UserAttributes { get; set; }

    public MessageResponse(string messageId, string receiptHandle, string body, List<QueueAttribute> attributes, List<MessageUserAttribute>? userAttributes)
    {
        MessageId = messageId;
        ReceiptHandle = receiptHandle;
        MD5OfBody = body.AsMD5Hash();
        Body = body;
        Attributes = attributes;
        if (userAttributes != null && userAttributes.Count > 0)
        {
            UserAttributes = userAttributes.Select(x => new MessageUserAttributeOut(x)).ToList();
            MD5OfMessageAttributes = MessageUserAttributesHashCalculator.Calculate(userAttributes);
        }
        else
        {
            UserAttributes = null;
            MD5OfMessageAttributes = null;
        }
    }

    public MessageResponse()
    {
    }

    public SendMessageResult ToSendResult() => new()
    {
        MD5OfBody = MD5OfBody,
        MessageId = MessageId,
        MD5OfMessageAttributes = MD5OfMessageAttributes
    };

    [JsonIgnore]
    public object JsonResult => new MessageResponseJson(this);
}

public class MessageResponseJson
{
    public Dictionary<string, string> Attributes { get; set; } = new();
    public string Body { get; set; }
    public string MD5OfBody { get; set; }
    public string? MD5OfMessageAttributes { get; set; }
    public Dictionary<string, MessageUserAttributeJson> MessageAttributes { get; set; } = new();
    public string MessageId { get; set; }
    public string ReceiptHandle { get; set; }
    
    public MessageResponseJson(){}

    public MessageResponseJson(MessageResponse source)
    {
        source.Attributes.ForEach(x => Attributes.Add(x.Name, x.Value));
        Body = source.Body;
        MD5OfBody = source.MD5OfBody;
        MD5OfMessageAttributes = source.MD5OfMessageAttributes;
        source.UserAttributes?.ForEach(x => MessageAttributes.Add(x.Name, new(x.Value.BinaryValue, x.Value.StringValue, x.Value.DataType)));
        MessageId = source.MessageId;
        ReceiptHandle = source.ReceiptHandle;
    }
}