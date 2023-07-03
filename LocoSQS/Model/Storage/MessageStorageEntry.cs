using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;

namespace LocoSQS.Model.Storage
{
    public class MessageStorageEntry : IMessageStorageModel
    {
        public string Id { get; }

        public string Body { get; }

        public string Receipt { get; }

        public List<MessageUserAttribute> UserAttributes { get; } = new();

        public Dictionary<string, string> Attributes { get; } = new();

        public MessageStorageEntry()
        {
        }

        public MessageStorageEntry(IMessage message) : this(message.Id, message.Body, message.Receipt, message.UserAttributes, message.ConstructAttributes(new()
        {
            "ApproximateFirstReceiveTimestamp",
            "ApproximateReceiveCount",
            "SentTimestamp",
            "DeadLetterQueueSourceArn"
        }))
        {
        }

        public MessageStorageEntry(string id, string body, string receipt, List<MessageUserAttribute> userAttributes, Dictionary<string, string> attributes)
        {
            Id = id;
            Body = body;
            Receipt = receipt;
            UserAttributes = userAttributes;
            Attributes = attributes;
        }
    }
}
