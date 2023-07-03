namespace LocoSQS.Model.ActionResults.Properties;

public class Tag
{
    public string Key { get; set; }
    public string Value { get; set; }

    public Tag()
    {
    }

    public Tag(string key, string value)
    {
        Key = key;
        Value = value;
    }
}