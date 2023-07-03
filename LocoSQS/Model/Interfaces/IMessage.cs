using LocoSQS.Model.ActionResults.Properties;
using LocoSQS.Model.Storage;
using LocoSQS.Model.Utils;

namespace LocoSQS.Model.Interfaces
{
    public interface IMessage
    {
        string Id { get; }
        string Body { get; }
        string Receipt { get; }
        public List<MessageUserAttribute> UserAttributes { get; }
        bool IsAvailable { get; }
        event Action<DateTimeOffset, IMessage> OnMessageReady;
        event Action<IMessage> OnMessageInvisible;
        event Action<IMessage> OnMessageDeleted;

        MessageResponse ToResponse(List<string>? requestedAttributes = null, List<string>? requestedUserAttributes = null);
        Dictionary<string, string> ConstructAttributes(List<string> items);
    }
}
