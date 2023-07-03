using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using Newtonsoft.Json;

namespace LocoSQS.Model.QueueJsonActions;

public class ChangeMessageVisibilityBatch : IQueueJsonAction
{
    [JsonProperty(Required = Required.Always)]
    public List<ChangeMessageVisibilityBatchEntry> Entries { get; set; } = new();

    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.ChangeMessageVisibilityBatch(Entries.Select(x => new ChangeMessageVisibilityBatchRequestEntry()
        {
            Id = x.Id,
            ReceiptHandle = x.ReceiptHandle,
            VisibilityTimeout = x.VisibilityTimeout
        }).ToList());
}

public class ChangeMessageVisibilityBatchEntry
{
    [JsonProperty(Required = Required.Always)]
    public string Id { get; set; } = "";
    
    [JsonProperty(Required = Required.Always)]
    public string ReceiptHandle { get; set; } = "";

    [JsonProperty(Required = Required.Always)]
    public int VisibilityTimeout { get; set; } = 30;
}