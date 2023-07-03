using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Storage;

namespace LocoSQS.Storage
{
    public class MemoryStorage : IQueueStorage
    {
        public event Action<IQueueStorageModel>? OnUpdateQueue;
        public event Action<IQueueStorageModel>? OnDeleteQueue;
        public event Action<long, IMessageStorageModel>? OnUpdateMessage;
        public event Action<long, IMessageStorageModel>? OnDeleteMessage;

        public void DeleteMessage(long queueId, IMessageStorageModel entry) => OnDeleteMessage?.Invoke(queueId, entry);

        public void DeleteQueue(IQueueStorageModel entry) => OnDeleteQueue?.Invoke(entry);

        public Dictionary<long, List<IMessageStorageModel>> GetMessages() => new();

        public List<IQueueStorageModel> GetQueues() => new();

        public void UpdateMessage(long queueId, IMessageStorageModel entry) => OnUpdateMessage?.Invoke(queueId, entry);

        public void UpdateQueue(IQueueStorageModel entry) => OnUpdateQueue?.Invoke(entry);
    }
}
