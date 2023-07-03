using LocoSQS.Model.Utils;

namespace LocoSQS.Model.Interfaces
{
    public interface IMessageStorageModel
    {
        string Id { get; }
        string Body { get; }
        string Receipt { get; }
        public List<MessageUserAttribute> UserAttributes { get; }
        public Dictionary<string, string> Attributes { get; }
    }
}
