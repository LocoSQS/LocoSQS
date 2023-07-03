using LocoSQS.Exeptions;
using System.Reflection;

namespace LocoSQS.Parser.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class DataProperty : Attribute, CustomAttribute
{
    public int Order => 0;
    public string? PropertyName { get; private set; } = null;
    public bool Required { get; } = true;

    public DataProperty(string? propertyName, bool required)
    {
        PropertyName = propertyName;
        Required = required;
    }
    
    public DataProperty(bool required)
    {
        Required = required;
    }
    
    public DataProperty(string? propertyName)
    {
        PropertyName = propertyName;
    }

    public DataProperty()
    {
    }

    public string GetName(PropertyInfo prop)
    {
        string? name = PropertyName;
        name ??= prop.Name;
        return name.ToLower();
    }

    public bool Apply(PropertyInfo prop, IDictionary<string, string> data, object o)
    {
        bool exists = data.Keys.Any(x => x.StartsWith(GetName(prop)));
        if (Required && !exists)
            throw new MissingParameter($"Property {GetName(prop)} is required");

        return exists;
    }
}