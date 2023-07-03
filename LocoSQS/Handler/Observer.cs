using LocoSQS.Model.ActionResults;
using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;
using LocoSQS.Queue;

namespace LocoSQS.Handler
{
    public class Observer
    {
        public IQueue Queue { get; private set; }
        private List<ObserverLog> _observers = new();

        public event Action<ObserverLog>? OnUpdate;

        public Observer(IQueue queue)
        {
            Queue = queue;

            Queue.OnNewRegisteredMessage += (x, y) =>
            {
                y.OnMessageDeleted += OnMessageDeleted;
                y.OnMessageReady += OnMessageReady;
                y.OnMessageInvisible += OnMessageInvisible;
                if (y.IsAvailable)
                    OnMessageReady(DateTimeOffset.Now, y);
                else
                    OnMessageInvisible(y);
            };
        }

        public IEnumerable<ObserverLog> GetLogs(int max = 50)
        {
            List<string> messageIds = _observers.TakeLast(max).Select(x => x.MessageId).Distinct().ToList();
            return _observers.Where(x => messageIds.Contains(x.MessageId));
        }

        private void OnMessageDeleted(IMessage message)
            => OnUpdateInternal(message, "OnMessageDeleted", DateTimeOffset.Now);

        private void OnMessageInvisible(IMessage message)
            => OnUpdateInternal(message, "OnMessageInvisible", DateTimeOffset.Now);

        private void OnMessageReady(DateTimeOffset time, IMessage message)
            => OnUpdateInternal(message, "OnMessageReady", time);

        private void OnUpdateInternal(IMessage message, string eventName, DateTimeOffset time)
        {
            ActionResult<MessageResponse> response = new(200, message.ToResponse(new() { "All" }, new() { "All" }));
            ObserverLog log = new(response.Value.MessageId, response.Json, time, eventName);

            lock (_observers)
            {
                _observers.Add(log);
            }

            OnUpdate?.Invoke(log);
        }
    }
}
