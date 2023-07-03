using LocoSQS.Exeptions;
using LocoSQS.Model.ActionResults;
using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Storage;
using LocoSQS.Model.Utils;
using Newtonsoft.Json;

namespace LocoSQS.Queue.Standard
{
    public class StandardQueue : IQueue
    {
        public long Id { get; }

        public string Name { get; }
        public IQueue AsIQueue => this;

        public event Action<IQueue, IMessage>? OnNewRegisteredMessage;
        public event Action<IQueue>? OnUpdate;

        public IQueueHandler QueueHandler { get; private set; }

        public List<StandardPool> Pools { get; set; } = new()
        {
            new(),
            new(),
            new(),
            new(),
            new(),
            new()
        };

        public DateTimeOffset CreatedTimestamp { get; set; } = DateTime.Now;
        public TimeSpan DelaySeconds { get; set; } = TimeSpan.Zero;
        public DateTimeOffset LastModifiedTimestamp { get; set; } = DateTime.Now;
        public int MaximumMessageSize { get; set; } = 0x40000; // 256 KiB
        public TimeSpan MessageRetentionPeriod { get; set; } = TimeSpan.FromDays(4);
        public string Policy { get; set; } = "{}";
        public TimeSpan ReceiveMessageWaitTimeSeconds { get; set; } = TimeSpan.Zero;
        public TimeSpan VisibilityTimeout { get; set; } = TimeSpan.FromSeconds(30);
        public RedrivePolicy? RedrivePolicy { get; set; } = null;
        public Dictionary<string,string> Tags { get; set; } = new();

        public ActionResult<AddPermissionResponse> AddPermission(string label, AddPermissionData newPermissions)
        {
            // Mocked: Not implemented
            return new(200, new());
        }

        public ActionResult<ChangeMessageVisibilityResponse> ChangeMessageVisibility(string receiptHandle, TimeSpan visibilityTimeout)
        {
            foreach (var pool in Pools)
            {
                if (pool.UpdateVisibilityViaReceipt(receiptHandle, visibilityTimeout))
                    return new(200, new());
            }

            throw new SQSException("The specified receipt handle isn't valid.", 400, "ReceiptHandleIsInvalid");
        }

        public ActionResult<DeleteMessageResponse> DeleteMessage(string receiptHandle)
        {
            foreach (var pool in Pools)
            {
                if (pool.DeleteMessageViaReceipt(receiptHandle))
                    return new(200, new());
            }

            throw new SQSException("The specified receipt handle isn't valid.", 400, "ReceiptHandleIsInvalid");
        }

        public TimeSpan? GetAgeOfOldestMessage()
        {
            DateTimeOffset? min = Pools.Select(x => x.MinCreatedDate()).Min();
            if (min == null)
                return null;

            return DateTimeOffset.Now - min;
        }

        public long GetNumberOfMessagesDelayed() => Pools.Select(p => p.Count(x => !x.IsAvailable && x.ReceiveCount <= 0)).Sum();
        public long GetNumberOfMessagesNotVisible() => Pools.Select(p => p.Count(x => !x.IsAvailable && x.ReceiveCount > 0)).Sum();
        public long GetNumberOfMessagesVisible() => Pools.Select(p => p.Count(x => x.IsAvailable)).Sum();
        private long GetNumberOfMessages() => Pools.Sum(x => x.MessageCount);

        public ActionResult<GetQueueAttributesResponse> GetQueueAttributes(List<string> attributes)
        {
            Dictionary<string, string> items = new();
            bool all = attributes.Contains("All");

            void GetQueueAttributeHelper(string key, Func<string> value)
            {
                if (all || attributes.Contains(key))
                    items.Add(key, value());
            }

            GetQueueAttributeHelper("ApproximateNumberOfMessages", () => GetNumberOfMessages().ToString());    
            GetQueueAttributeHelper("ApproximateNumberOfMessagesDelayed", () => GetNumberOfMessagesDelayed().ToString());    
            GetQueueAttributeHelper("ApproximateNumberOfMessagesNotVisible", () => GetNumberOfMessagesNotVisible().ToString());    
            GetQueueAttributeHelper("CreatedTimestamp", () => CreatedTimestamp.ToUnixTimeSeconds().ToString());    
            GetQueueAttributeHelper("DelaySeconds", DelaySeconds.TotalSeconds.ToString);    
            GetQueueAttributeHelper("LastModifiedTimestamp", () => LastModifiedTimestamp.ToUnixTimeSeconds().ToString());    
            GetQueueAttributeHelper("MaximumMessageSize", MaximumMessageSize.ToString);
            GetQueueAttributeHelper("Policy", Policy.ToString);
            GetQueueAttributeHelper("MessageRetentionPeriod", MessageRetentionPeriod.TotalSeconds.ToString);
            GetQueueAttributeHelper("QueueArn", () => AsIQueue.Arn);    
            GetQueueAttributeHelper("ReceiveMessageWaitTimeSeconds", ReceiveMessageWaitTimeSeconds.TotalSeconds.ToString);    
            GetQueueAttributeHelper("VisibilityTimeout", VisibilityTimeout.TotalSeconds.ToString);   
            
            if (RedrivePolicy != null)
            {
                GetQueueAttributeHelper("RedrivePolicy", RedrivePolicy.ToString);
            }

            GetQueueAttributeHelper("SqsManagedSseEnabled", () => "false");

            return new(200, new(items));
        }

        public ActionResult<ListQueueTagsResponse> ListQueueTags()
        {
            ListQueueTagsResponse response;
            lock (Tags)
            {
               response = new(Tags);
            }

            return new(200, response);
        }

        private DateTimeOffset _lastPurgeQueue = DateTimeOffset.Now - TimeSpan.FromMinutes(5);
        public ActionResult<PurgeQueueResponse> PurgeQueue(bool ignoreTime = false)
        {
            if (!ignoreTime && (_lastPurgeQueue + TimeSpan.FromMinutes(1)) > DateTimeOffset.Now)
                throw new SQSException($"Only one PurgeQueue operation on {Name} is allowed every 60 seconds.", 403, "AWS.SimpleQueueService.PurgeQueueInProgress");

            foreach (var pool in Pools)
            {
                pool.Clear();
            }

            _lastPurgeQueue = DateTimeOffset.Now;
            return new(200, new());
        }

        public ActionResult<ReceiveMessageResponse> ReceiveMessage(List<string>? attributes, int? maxNumberOfMessages, List<string>? messageAttributeNames, string? receiveRequestAttemptId, int? visibilityTimeout, int? waitTimeSeconds, bool? silent)
        {
            int max = maxNumberOfMessages ?? 1;
            TimeSpan visibility = (visibilityTimeout != null) ? TimeSpan.FromSeconds(visibilityTimeout.Value) : VisibilityTimeout;
            TimeSpan wait = (waitTimeSeconds != null)
                ? TimeSpan.FromSeconds(waitTimeSeconds.Value)
                : ReceiveMessageWaitTimeSeconds;
            
            if (silent ?? false)
                visibility = TimeSpan.Zero;

            List<Message> messages = new();

            if (wait == TimeSpan.Zero)
            {
                messages = GetRandomPool().GetMessages(max, visibility, silent ?? false);
            }
            else
            {
                DateTimeOffset end = DateTimeOffset.Now + wait;
                while (end > DateTimeOffset.Now)
                {
                    messages = GetPoolWithMessage().GetMessages(max, visibility, silent ?? false);
                    if (messages.Count > 0)
                        break;
                    
                    Thread.Sleep(100);
                }
            }
            
            List<Message> dlqMessages = messages.Where(x => RedrivePolicy != null && RedrivePolicy.MaxReceiveCount < x.ReceiveCount).ToList();

            if (dlqMessages.Count > 0)
            {
                messages.RemoveAll(dlqMessages.Contains);
                dlqMessages.ForEach(x => DeleteMessage(x.Receipt));
                IQueue? queue = QueueHandler.GetQueueViaArn(RedrivePolicy!.DeadLetterTargetArn);

                if (queue != null)
                {
                    dlqMessages.ForEach(x =>
                    {
                        x.DeadLetterQueueSourceArn = AsIQueue.Arn;
                        queue.AddMessage(x);
                    });
                }
            }

            return new(200, new(messages.Select(x => x.ToResponse(attributes, messageAttributeNames)).ToList()));
        }

        public ActionResult<RemovePermissionResponse> RemovePermission(string label)
        {
            // Mocked: Not implemented
            return new(200, new());
        }

        public ActionResult<SendMessageResponse> SendMessage(int? delaySeconds, List<MessageUserAttribute> attributes, string messageBody, string? messageDeduplicationId, string? messageGroupId)
        {
            if (messageBody.Length + attributes.Sum(x => x.Length()) > MaximumMessageSize)
                throw new ValidationError($"Message exceeds length of {MaximumMessageSize} bytes");

            Message message = new(messageBody,
                (delaySeconds.HasValue) ? TimeSpan.FromSeconds(delaySeconds.Value) : DelaySeconds, attributes);
            OnNewRegisteredMessage?.Invoke(this, message);

            return new(200, new(message));
        }

        public ActionResult<SetQueueAttributesResponse> SetQueueAttributes(Dictionary<string, string> attributes)
        {
            Configure(attributes);
            OnUpdate?.Invoke(this);
            return new(200, new());
        }

        public ActionResult<TagQueueResponse> TagQueue(Dictionary<string, string> tags)
        {
            lock (Tags)
            {
                foreach (var (key, value) in tags)
                {
                    Tags[key] = value;
                }
            }

            OnUpdate?.Invoke(this);
            
            return new(200, new());
        }

        public ActionResult<UntagQueueResponse> UntagQueue(List<string> tags)
        {
            lock (Tags)
            {
                foreach (string tag in tags)
                {
                    Tags.Remove(tag);
                }
            }
            
            OnUpdate?.Invoke(this);

            return new(200, new());
        }

        public void SetMessages(List<IMessageStorageModel>? messages)
        {
            if (messages == null)
                return;

            foreach (var message in messages)
            {
                Message parsedMessage = new(message.Id, message.Body, message.Receipt, TimeSpan.Zero, message.UserAttributes, message.Attributes);
                OnNewRegisteredMessage?.Invoke(this, parsedMessage);
            }
        }

        public StandardQueue(QueueStorageEntry queue, IQueueStorage storage) : this(queue, (IQueueHandler)null)
        {}

        public StandardQueue(IQueueStorageModel queue, IQueueHandler handler) : this(queue.Id, queue.Name, queue.Attributes, queue.Tags, handler)
        {}

        public StandardQueue(long id, string name, Dictionary<string, string> attributes, Dictionary<string, string> tags, IQueueHandler handler)
        {
            QueueHandler = handler;
            Id = id;
            Name = name;
            Tags = tags;

            Configure(attributes);

            OnNewRegisteredMessage += (q, m) =>
            {
                AddMessage(m as Message);
            };
        }

        private void AddMessage(Message message)
        {
            GetRandomPool().Add(message);
        }
        
        private StandardPool GetRandomPool() => Pools[Random.Shared.Next(Pools.Count)];
        private StandardPool GetPoolWithMessage() => Pools.ToList().OrderBy(x => Random.Shared.Next()).FirstOrDefault(x => x.ContainsMessage, GetRandomPool());

        private void Configure(Dictionary<string, string> attributes)
        {
            int QueueConstructorIntParse(string key, string value, int min, int max)
            {
                int s = int.Parse(value);

                if (s < min || s > max)
                    throw new InvalidQueryParameter($"{key} property can only be between {min} and {max}");

                return s;
            }

            void QueueConstructorHelper(string key, Action<string> set)
            {
                if (attributes.ContainsKey(key))
                    set(attributes[key]);
            }
            
            QueueConstructorHelper("DelaySeconds", x => DelaySeconds = TimeSpan.FromSeconds(QueueConstructorIntParse("DelaySeconds", x, 0, 900))); // Between 0 and 15 minutes
            QueueConstructorHelper("MaximumMessageSize", x => MaximumMessageSize = QueueConstructorIntParse("MaximumMessageSize", x, 0x400, 0x40000)); // Between 1 and 256 KiB
            QueueConstructorHelper("MessageRetentionPeriod", x => MessageRetentionPeriod = TimeSpan.FromSeconds(QueueConstructorIntParse("MessageRetentionPeriod", x, 60, 1209600))); // 1 minute to 14 days
            QueueConstructorHelper("Policy", x => Policy = x);
            QueueConstructorHelper("ReceiveMessageWaitTimeSeconds", x => ReceiveMessageWaitTimeSeconds = TimeSpan.FromSeconds(QueueConstructorIntParse("ReceiveMessageWaitTimeSeconds", x, 0, 20))); // 0 to 20 seconds
            QueueConstructorHelper("VisibilityTimeout", x => VisibilityTimeout = TimeSpan.FromSeconds(QueueConstructorIntParse("VisibilityTimeout", x, 0, 43200))); // 0 to 12 hours
            QueueConstructorHelper("RedrivePolicy", x => RedrivePolicy = JsonConvert.DeserializeObject<RedrivePolicy>(x));

            if (RedrivePolicy != null && (RedrivePolicy.MaxReceiveCount == 0 || RedrivePolicy.DeadLetterTargetArn == null))
                RedrivePolicy = null;
        }
        
        void IQueue.AddMessage(Message message)
            => OnNewRegisteredMessage?.Invoke(this, message);
    }
}
