using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.Utils;

public class MessageUserAttributeIn
{
    [DataProperty]
    [AsString(256)]
    public string Name { get; set; }
    
    [DataProperty]
    [AsObject]
    public MessageUserAttributeInValue Value { get; set; }

    public MessageUserAttribute ToMessageUserAttribute()
        => new()
        {
            Name = Name,
            BinaryValue = Value.BinaryValue,
            StringValue = Value.StringValue,
            DataType = Value.DataType
        };
}

public class MessageUserAttributeInValue
{
    [DataProperty]
    [AsString(new string[3] { "String", "Number", "Binary" }, true)]
    public string DataType { get; set; }
    
    [DataProperty(false)]
    [AsString(256)]
    public string? StringValue { get; set; }
    
    [DataProperty(false)]
    [AsString(256)]
    public string? BinaryValue { get; set; }
}