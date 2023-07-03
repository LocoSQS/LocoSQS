using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Parser.Attributes;
using Newtonsoft.Json;

namespace LocoSQS.Model.QueueActions
{
    public class ChangeMessageVisibility : IQueueAction, IQueueJsonAction
    {
        [DataProperty]
        [AsInt(0, 43200)]
        [JsonProperty(Required = Required.Always)]
        public int VisibilityTimeout { get; set; }

        [DataProperty]
        [AsString]
        [JsonProperty(Required = Required.Always)]
        public string ReceiptHandle { get; set; }

        public ActionResult Run(IQueueHandler handler, IQueue queue)
            => queue.ChangeMessageVisibility(ReceiptHandle, TimeSpan.FromSeconds(VisibilityTimeout));
    }
}
