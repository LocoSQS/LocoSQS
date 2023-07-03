using LocoSQS.Exeptions;
using LocoSQS.Model.ActionResults;
using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Queue.Standard;

namespace LocoSQS.Handler
{
    public class QueueHandler : IQueueHandler
    {
        public event Action<IQueue>? OnNewQueue;
        public event Action<IQueue>? OnDeleteQueue;
        public List<IQueue> Queues { get; } = new();
        public IQueueStorage Storage { get; private set; }
        public bool ReadOnly { get; set; } = true;

        public ActionResult<CreateQueueResponse> CreateQueue(Dictionary<string, string> attributes, string queueName, Dictionary<string, string> tags)
        {
            if (GetQueue(queueName, false) != null)
                throw new SQSException("A queue with this name already exists", 400, "QueueAlreadyExists");
            
            IQueue newQueue;

            if (queueName.EndsWith(".fifo"))
                throw new NotImplementedException("Fifo queues are not implemented");
            else
                newQueue = new StandardQueue(Random.Shared.NextInt64(), queueName, attributes, tags, this);

            OnNewQueue?.Invoke(newQueue);
            return new ActionResult<CreateQueueResponse>(200, new(newQueue.Url));
        }

        public ActionResult<DeleteQueueResponse> DeleteQueue(string queueName)
        {
            IQueue? queue = GetQueue(queueName);

            if (queue == null)
                throw new NotAuthorized();
            
            OnDeleteQueue?.Invoke(queue);
            return new(200, new());
        }

        public IQueue? GetQueue(string queueName) => GetQueue(queueName, true);
        public IQueue? GetQueue(string queueName, bool withRecursion)
        {
            lock (Queues)
            {
                foreach (var queue in Queues)
                {
                    if (string.Equals(queue.Name, queueName, StringComparison.CurrentCultureIgnoreCase))
                        return queue;
                    
                    if (string.Equals(queue.Url, queueName, StringComparison.CurrentCultureIgnoreCase))
                        return queue;
                }
            }

            if (ServerConfiguration.CREATEQUEUEIMPLICITLY && withRecursion)
            {
                CreateQueue(new(), queueName, new());
                return GetQueue(queueName, false);
            }

            return null;
        }

        public ActionResult<GetQueueUrlResponse> GetQueueUrl(string queueName, string? queueOwnerAWSAccountId)
        {
            IQueue? queue = GetQueue(queueName);
            if (queue == null)
                throw new SQSException("The specified queue doesn't exist.", 400,
                    "AWS.SimpleQueueService.NonExistentQueue");
            
            return new(200, new(queue.Url));
        }

        public ActionResult<ListDeadLetterSourceQueuesResponse> ListDeadLetterSourceQueues(IQueue dlq, int? maxResults, string? nextToken)
        {
            string dlqArn = dlq.Arn;
            List<IQueue> sourceQueues;

            lock (Queues)
            {
                sourceQueues = Queues.Where(x => x.RedrivePolicy?.DeadLetterTargetArn == dlqArn).ToList();
            }

            int max = maxResults ?? 1000;
            int page = int.Parse(nextToken ??= "1");
            int skip = (page - 1) * max;
            
            List<string> queueUrls = sourceQueues.Skip(skip).Take(max).Select(x => x.Url).ToList();
            
            return new(200, new(queueUrls, sourceQueues.Count >= (page * max) ? (page + 1).ToString() : null));
        }

        public ActionResult<ListQueuesResponse> ListQueues(int? maxResults, string? nextToken, string? queueNamePrefix)
        {
            int count;
            IEnumerable<string> queues;

            lock (Queues)
            {
                queues = Queues.Select(x => x.Url);
                count = Queues.Count;
            }

            if (!string.IsNullOrWhiteSpace(queueNamePrefix))
                queues = queues.Where(x => x.StartsWith(queueNamePrefix));
            
            bool nextTokenNeeded = false;
            int page = 1;

            if (maxResults.HasValue)
            {
                page = int.Parse(nextToken ?? "1");
                queues = queues.Skip(maxResults!.Value * (page - 1)).Take(maxResults!.Value);
                nextTokenNeeded = ((count + maxResults - 1) / maxResults > page);
            }

            return new(200, new(queues.ToList(), (nextTokenNeeded) ? (page + 1).ToString() : null));
        }

        public void Init(IQueueStorage storage, bool readOnly = false)
        {
            var queues = storage.GetQueues();
            var messages = storage.GetMessages();
            queues.ForEach(q =>
            {
                if (q.Name.EndsWith(".fifo"))
                    throw new NotImplementedException("Fifo queues are not implemented");
                else
                {
                    StandardQueue queue = new StandardQueue(q, this);
                    OnNewQueue?.Invoke(queue);
                    queue.SetMessages(messages.ContainsKey(queue.Id) ? messages[queue.Id] : null);
                }
            });

            ReadOnly = readOnly;
            Storage = storage;
        }

        private void OnNewQueueInternal(IQueue queue)
        {
            ServerConfiguration.DebugLog($"OnNewQueueInternal: {queue.Name}");

            lock (Queues)
            {
                Queues.Add(queue);
            }
            
            if (!ReadOnly)
                Storage.UpdateQueue(queue);

            queue.OnNewRegisteredMessage += OnNewRegisteredMessage;
            queue.OnUpdate += OnQueueUpdate;
        }

        private void OnDeleteQueueInternal(IQueue queue)
        {
            ServerConfiguration.DebugLog($"OnDeleteQueueInternal: {queue.Name}");
            
            lock (Queues)
            {
                Queues.Remove(queue);
            }
            
            if (!ReadOnly)
                Storage.DeleteQueue(queue);
            
            queue.OnNewRegisteredMessage -= OnNewRegisteredMessage;
            queue.OnUpdate -= OnQueueUpdate;
        }
        
        private void OnQueueUpdate(IQueue queue)
        {
            ServerConfiguration.DebugLog($"OnQueueUpdate: {queue.Name}");

            if (!ReadOnly)
                Storage.UpdateQueue(queue);
        }

        private void OnNewRegisteredMessage(IQueue queue, IMessage message)
        {
            ServerConfiguration.DebugLog($"OnNewRegisteredMessage: {queue.Name} -> {message.Body}");
            message.OnMessageDeleted += x => OnMessageDeleted(queue, x);
            message.OnMessageInvisible += x => OnMessageInvisible(queue, x);

            if (!ReadOnly)
                Storage.UpdateMessage(queue.Id, message);
        }

        private void OnMessageInvisible(IQueue queue, IMessage message)
        {
            ServerConfiguration.DebugLog($"OnMessageInvisible: {queue.Name} -> {message.Body}");

            if (!ReadOnly)
                Storage.UpdateMessage(queue.Id, message);
        }

        private void OnMessageDeleted(IQueue queue, IMessage message)
        {
            ServerConfiguration.DebugLog($"OnMessageDeleted: {queue.Name} -> {message.Body}");

            if (!ReadOnly)
                Storage.DeleteMessage(queue.Id, message);
        }

        public IQueue? GetQueueViaArn(string queueArn)
        {
            foreach (var queue in Queues)
            {
                if (string.Equals(queue.Arn, queueArn, StringComparison.CurrentCultureIgnoreCase))
                    return queue;
            }

            return null;
        }

        public QueueHandler()
        {
            OnNewQueue += OnNewQueueInternal;
            OnDeleteQueue += OnDeleteQueueInternal;
        }
    }
}
