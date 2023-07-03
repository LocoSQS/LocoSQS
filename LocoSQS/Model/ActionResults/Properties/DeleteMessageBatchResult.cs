using System.Xml.Serialization;
using Newtonsoft.Json;

namespace LocoSQS.Model.ActionResults.Properties
{
    public class DeleteMessageBatchResult
    {
        [XmlElement("DeleteMessageBatchResultEntry")]
        [JsonProperty("Successful")]
        public List<DeleteMessageBatchResultEntry> Entries { get; set; } = new();

        [XmlElement("BatchResultErrorEntry")]
        [JsonProperty("Failed")]
        public List<BatchResultErrorEntry> Errors { get; set; } = new();


        public DeleteMessageBatchResult() { }
        public DeleteMessageBatchResult(List<DeleteMessageBatchResultEntry> entries)
        {
            Entries = entries;
        }

        public DeleteMessageBatchResult(List<DeleteMessageBatchResultEntry> entries, List<BatchResultErrorEntry> errors) : this(entries)
        {
            Errors = errors;
        }
    }
}
