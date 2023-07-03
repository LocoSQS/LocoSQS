using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Storage;
using Newtonsoft.Json;

namespace LocoSQS.Storage.Json
{
    public class JsonStorage : IQueueStorage
    {
        public event Action<IQueueStorageModel>? OnUpdateQueue;
        public event Action<IQueueStorageModel>? OnDeleteQueue;
        public event Action<long, IMessageStorageModel>? OnUpdateMessage;
        public event Action<long, IMessageStorageModel>? OnDeleteMessage;
        public List<JsonQueue> Storage { get; private set; } = new();
        private bool _dueForSaving = false;

        public List<IQueueStorageModel> GetQueues()
            => Storage.Select(x => (IQueueStorageModel)x).ToList();

        public void UpdateQueue(IQueueStorageModel entry)
        {
            lock (Storage)
            {
                JsonQueue? storage = Storage.FirstOrDefault(x => x.Id == entry.Id);

                if (storage == null)
                {
                    storage = new JsonQueue(entry);
                    Storage.Add(storage);
                    _dueForSaving = true;
                }
                else
                {
                    lock (storage)
                    {
                        storage.Overwrite(entry);
                        _dueForSaving = true;
                    }
                }

                OnUpdateQueue?.Invoke(storage);
            }
        }

        public void DeleteQueue(IQueueStorageModel entry)
        {
            lock (Storage)
            {
                JsonQueue? storage = Storage.FirstOrDefault(x => x.Id == entry.Id);

                if (storage == null)
                    return;

                lock (storage)
                {
                    Storage.Remove(storage);
                    _dueForSaving = true;

                    OnDeleteQueue?.Invoke(storage);
                }
            }    
        }

        public Dictionary<long, List<IMessageStorageModel>> GetMessages()
        {
            Dictionary<long, List<IMessageStorageModel>> messages = new();
            Storage.ForEach(x => messages.Add(x.Id, x.Messages.Select(x => (IMessageStorageModel)x).ToList()));
            return messages;
        }

        public void UpdateMessage(long queueId, IMessageStorageModel entry)
        {
            lock (Storage)
            {
                JsonQueue? storage = Storage.FirstOrDefault(x => x.Id == queueId);

                if (storage == null)
                    return;

                lock(storage)
                {
                    JsonMessage? message = storage.Messages.FirstOrDefault(x => x.Id == entry.Id);

                    if (message == null)
                    {
                        message = new(entry);
                        storage.Messages.Add(message);
                        _dueForSaving = true;
                    }
                    else
                    {
                        lock (message)
                        {
                            message.Overwrite(entry);
                            _dueForSaving = true;
                        }
                    }

                    OnUpdateMessage?.Invoke(storage.Id, message);
                }
            }
        }

        public void DeleteMessage(long queueId, IMessageStorageModel entry)
        {
            lock (Storage)
            {
                JsonQueue? storage = Storage.FirstOrDefault(x => x.Id == queueId);

                if (storage == null)
                    return;

                lock (storage)
                {
                    JsonMessage? message = storage?.Messages.FirstOrDefault(x => x.Id == entry.Id);

                    if (message == null)
                        return;

                    lock (message)
                    {
                        storage.Messages.Remove(message);
                        _dueForSaving = true;

                        OnDeleteMessage?.Invoke(storage.Id, message);
                    }
                }
            }
        }

        private void Write()
        {
            lock (Storage)
            {
                lock (_path)
                {
                    string data = JsonConvert.SerializeObject(Storage);
                    File.WriteAllText(_path, data);
                }
            }
        }

        private string _path;
        private Timer _saveTimer;

        public JsonStorage(string path)
        {
            _path = path;
            if (File.Exists(path))
            {
                string text = File.ReadAllText(_path);
                Storage = JsonConvert.DeserializeObject<List<JsonQueue>>(text)!;
            }

            _saveTimer = new(_ => ContinousSave(), null, TimeSpan.Zero, TimeSpan.FromSeconds(5));
            AppDomain.CurrentDomain.ProcessExit += (_, _) => ContinousSave();
        }

        private void ContinousSave()
        {
            if (_dueForSaving)
            {
                _dueForSaving = false;
                Write();
            }
        }
    }
}
