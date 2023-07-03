using System.Xml.Serialization;

namespace LocoSQS.Model.Utils
{
    public class MessageUserAttributeOut
    {
        public string Name { get; set; }
        public MessageUserAttributeOutValue Value { get; set; }

        public MessageUserAttributeOut()
        {
        }

        public MessageUserAttributeOut(MessageUserAttribute attribute)
        {
            Name = attribute.Name;
            Value = new()
            {
                StringValue = attribute.StringValue,
                BinaryValue = attribute.BinaryValue,
                DataType = attribute.DataType,
            };
        }
    }

    public class MessageUserAttributeOutValue
    {
        public string? StringValue { get; set; }
        public string? BinaryValue { get; set; }
        public string DataType { get; set; }
    }
}
