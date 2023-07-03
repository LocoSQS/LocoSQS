using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using Newtonsoft.Json;

namespace LocoSQS.Model.QueueJsonActions;

public class DeleteMessageBatch : IQueueJsonAction
{
    [JsonProperty(Required = Required.Always)]
    public List<DeleteMessageBatchEntry> Entries { get; set; } = new();


    public ActionResult Run(IQueueHandler handler, IQueue queue)
        => queue.DeleteMessageBatch(Entries.Select(x => new DeleteMessageBatchRequestEntry()
        {
            Id = x.Id,
            ReceiptHandle = x.ReceiptHandle
        }).ToList());
}

public class DeleteMessageBatchEntry
{
    [JsonProperty(Required = Required.Always)]
    public string Id { get; set; } = "";

    [JsonProperty(Required = Required.Always)]
    public string ReceiptHandle { get; set; } = "";
}