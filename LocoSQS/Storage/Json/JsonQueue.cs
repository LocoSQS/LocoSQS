using LocoSQS.Model.Interfaces;

namespace LocoSQS.Storage.Json
{
    public class JsonQueue : IQueueStorageModel
    {
        public long Id { get; set; }

        public string Name { get; set; }

        public Dictionary<string, string> Attributes { get; set; } = new();

        public Dictionary<string, string> Tags { get; set; } = new();

        public List<JsonMessage> Messages { get; set; } = new();

        public JsonQueue() { }

        public JsonQueue(IQueueStorageModel model)
        {
            Overwrite(model);
        }

        public void Overwrite(IQueueStorageModel model)
        {
            Id = model.Id;
            Name = model.Name;
            Attributes = model.Attributes;
            Tags = model.Tags;
        }
    }
}
