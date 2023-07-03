using System.Xml.Serialization;
using Newtonsoft.Json;

namespace LocoSQS.Model.ActionResults.Properties
{
    public class SendMessageBatchResultEntry
    {
        public string Id { get; set; }
        public string MessageId { get; set; }
        [JsonProperty("MD5OfMessageBody")]
        public string MD5OfBody { get; set; }
        public string? MD5OfMessageAttributes { get; set; }

        public SendMessageBatchResultEntry(string id, string messageId, string mD5OfBody, string? mD5OfMessageAttributes)
        {
            Id = id;
            MessageId = messageId;
            MD5OfBody = mD5OfBody;
            MD5OfMessageAttributes = mD5OfMessageAttributes;
        }

        public SendMessageBatchResultEntry()
        {
        }
    }
}
