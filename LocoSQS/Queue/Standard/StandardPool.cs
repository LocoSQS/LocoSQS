using System.Collections.Concurrent;

namespace LocoSQS.Queue.Standard;

public class StandardPool
{
    public bool ContainsMessage => Messages.Count > 0;
    public long MessageCount => Messages.Count;

    private List<Message> Messages { get; set; } = new();
    
    public List<Message> GetMessages(int limit, TimeSpan timeout, bool silent)
    {
        lock (Messages)
        {
            return Messages
                .Where(x => x.IsAvailable)
                .OrderBy(x => Random.Shared.Next())
                .Take(limit)
                .Select(x => x.Recieve(timeout, silent))
                .ToList();
        }
    }

    public void Clear()
    {
        lock (Messages)
        {
            foreach (Message m in Messages)
            {
                m.Delete();
            }

            Messages.Clear();
        }
    }

    public void Add(Message message)
    {
        lock (Messages)
        {
            Messages.Add(message);
        }
    }

    public bool UpdateVisibilityViaReceipt(string receipt, TimeSpan timeout)
    {
        lock (Messages)
        {
            Message? message = Messages.FirstOrDefault(x => x.Receipt == receipt);
            if (message == null)
                return false;

            lock (message)
            {
                message.UpdateVisibilityTimeout(timeout);
            }
        }
        return true;
    }

    public bool DeleteMessageViaReceipt(string receipt)
    {
        lock (Messages)
        {
            Message? message = Messages.FirstOrDefault(x => x.Receipt == receipt);
            if (message == null)
                return false;

            lock (message)
            {
                Messages.Remove(message);
                message.Delete();
            }
        }

        return true;
    }

    public DateTimeOffset MinCreatedDate()
    {
        lock (Messages)
        {
            return Messages.Min(x => x.Created);
        }
    }

    public long Count(Func<Message, bool> predicate)
    {
        lock(Messages) 
        { 
            return Messages.Count(predicate);
        }
    }
}