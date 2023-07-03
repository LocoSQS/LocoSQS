namespace LocoSQS.Model.Utils
{
    public class MessageUserAttribute
    {
        public string Name { get; set; }
        public string? StringValue { get; set; }
        public string? BinaryValue { get; set; }
        public string DataType { get; set; }

        public long Length()
        {
            return Name.Length + (StringValue?.Length ?? 0) + (BinaryValue?.Length ?? 0) + DataType.Length;
        }
    }
    
    public class MessageUserAttributeJson
    {
        public string? BinaryValue { get; set; }
        public string? StringValue { get; set; }
        public string DataType { get; set; }

        public MessageUserAttributeJson(string? binaryValue, string? stringValue, string dataType)
        {
            BinaryValue = binaryValue;
            StringValue = stringValue;
            DataType = dataType;
        }
    }
}
