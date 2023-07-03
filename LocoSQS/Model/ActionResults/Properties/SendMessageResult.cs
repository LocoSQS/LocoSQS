using Newtonsoft.Json;

namespace LocoSQS.Model.ActionResults.Properties;

public class SendMessageResult
{
    public string MessageId { get; set; }
    [JsonProperty("MD5OfMessageBody")]
    public string MD5OfBody { get; set; }
    public string? MD5OfMessageAttributes { get; set; }

    public SendMessageResult()
    {
    }
}