using LocoSQS.Model.Interfaces;

namespace LocoSQS.Model.Storage
{
    public class QueueStorageEntry : IQueueStorageModel
    {
        public long Id { get; }
        public string Name { get; }
        public Dictionary<string, string> Attributes { get; } = new();
        public Dictionary<string, string> Tags { get; } = new();

        public QueueStorageEntry()
        {
        }

        public QueueStorageEntry(IQueue queue)
        {
            Id = queue.Id;
            Name = queue.Name;
            queue.GetQueueAttributes(new()
            {
                "DelaySeconds",
                "MaximumMessageSize",
                "MessageRetentionPeriod",
                "Policy",
                "ReceiveMessageWaitTimeSeconds",
                "VisibilityTimeout",
                "RedrivePolicy",
                "RedriveAllowPolicy"
            }).Value.Attributes.ForEach(x => Attributes.Add(x.Name, x.Value));

            var tags = queue.ListQueueTags();
            tags.Value.ListQueueTagsResult.Tags.ForEach(x => Tags.Add(x.Key, x.Value));
        }

        public QueueStorageEntry(long id, string name, Dictionary<string, string> attributes, Dictionary<string,string> tags)
        {
            Id = id;
            Name = name;
            Attributes = attributes;
            Tags = tags;
        }
    }
}
