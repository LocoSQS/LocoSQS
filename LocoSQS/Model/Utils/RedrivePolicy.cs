using Newtonsoft.Json;

namespace LocoSQS.Model.Utils
{
    public class RedrivePolicy
    {
        [JsonProperty("deadLetterTargetArn")]
        public string DeadLetterTargetArn { get; set; }
        [JsonProperty("maxReceiveCount")]
        public int MaxReceiveCount { get; set; }

        public override string ToString() => JsonConvert.SerializeObject(this);
    }
}
