using LocoSQS.Model.Interfaces;
using LocoSQS.Model.Utils;

namespace LocoSQS.Storage.Json
{
    public class JsonMessage : IMessageStorageModel
    {
        public string Id { get; set; }

        public string Body { get; set; }

        public string Receipt { get; set; }

        public List<MessageUserAttribute> UserAttributes { get; set; }

        public Dictionary<string, string> Attributes { get; set; } = new();

        public JsonMessage() { }

        public JsonMessage(IMessageStorageModel model)
        {
            Overwrite(model);
        }

        public void Overwrite(IMessageStorageModel model)
        {
            Id = model.Id;
            Body = model.Body;
            Receipt = model.Receipt;
            UserAttributes = model.UserAttributes;
            Attributes = model.Attributes;
        }
    }
}
