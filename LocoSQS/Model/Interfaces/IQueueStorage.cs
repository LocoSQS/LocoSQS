using LocoSQS.Model.Storage;

namespace LocoSQS.Model.Interfaces
{
    public interface IQueueStorage
    {
        event Action<IQueueStorageModel> OnUpdateQueue;
        event Action<IQueueStorageModel> OnDeleteQueue;
        event Action<long, IMessageStorageModel> OnUpdateMessage;
        event Action<long, IMessageStorageModel> OnDeleteMessage;

        List<IQueueStorageModel> GetQueues();
        void UpdateQueue(IQueueStorageModel entry);
        void UpdateQueue(IQueue entry) => UpdateQueue(new QueueStorageEntry(entry));
        void DeleteQueue(IQueueStorageModel entry);
        void DeleteQueue(IQueue entry) => UpdateQueue(new QueueStorageEntry(entry));

        Dictionary<long, List<IMessageStorageModel>> GetMessages();
        void UpdateMessage(long queueId, IMessageStorageModel entry);
        void UpdateMessage(long queueId, IMessage message) => UpdateMessage(queueId, new MessageStorageEntry(message));
        void DeleteMessage(long queueId, IMessageStorageModel entry);
        void DeleteMessage(long queueId, IMessage message) => DeleteMessage(queueId, new MessageStorageEntry(message));
    }
}
