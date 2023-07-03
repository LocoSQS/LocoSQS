namespace LocoSQS.Model.Interfaces
{
    public interface IQueueStorageModel
    {
        public long Id { get; }
        public string Name { get; }
        public Dictionary<string, string> Attributes { get; }
        public Dictionary<string, string> Tags { get; }
    }
}
