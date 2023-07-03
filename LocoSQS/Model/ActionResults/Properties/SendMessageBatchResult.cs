using System.Xml.Serialization;
using Newtonsoft.Json;

namespace LocoSQS.Model.ActionResults.Properties
{
    public class SendMessageBatchResult
    {
        [XmlElement("ChangeMessageVisibilityBatchResultEntry")]
        [JsonProperty("Successful")]
        public List<SendMessageBatchResultEntry> Entries { get; set; } = new();

        [XmlElement("BatchResultErrorEntry")]
        [JsonProperty("Failed")]
        public List<BatchResultErrorEntry> Errors { get; set; } = new();
        
        public SendMessageBatchResult() { }
        public SendMessageBatchResult(List<SendMessageBatchResultEntry> entries)
        {
            Entries = entries;
        }

        public SendMessageBatchResult(List<SendMessageBatchResultEntry> entries, List<BatchResultErrorEntry> errors) : this(entries)
        {
            Errors = errors;
        }
    }
}
