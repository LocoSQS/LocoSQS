using System.Xml.Serialization;
using Newtonsoft.Json;

namespace LocoSQS.Model.ActionResults.Properties
{
    public class ChangeMessageVisibilityBatchResult
    {
        [XmlElement("ChangeMessageVisibilityBatchResultEntry")]
        [JsonProperty("Successful")]
        public List<ChangeMessageVisibilityBatchResultEntry> Entries { get; set; } = new();

        [XmlElement("BatchResultErrorEntry")]
        [JsonProperty("Failed")]
        public List<BatchResultErrorEntry> Errors { get; set; } = new();


        public ChangeMessageVisibilityBatchResult() { }
        public ChangeMessageVisibilityBatchResult(List<ChangeMessageVisibilityBatchResultEntry> entries)
        {
            Entries = entries;
        }

        public ChangeMessageVisibilityBatchResult(List<ChangeMessageVisibilityBatchResultEntry> entries, List<BatchResultErrorEntry> errors) : this(entries)
        {
            Errors = errors;
        }
    }
}
