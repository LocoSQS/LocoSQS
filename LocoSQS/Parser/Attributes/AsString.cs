using LocoSQS.Exeptions;
using System.Reflection;

namespace LocoSQS.Parser.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class AsString : Attribute, CustomAttribute
{
    public int Min { get; set; } = int.MinValue;
    public int Max { get; set; } = int.MaxValue;
    public List<string>? Choices { get; set; } = null;
    public bool ChoicesStartsWith { get; set; } = false;
    public int Order => 10;
    public bool Apply(PropertyInfo prop, IDictionary<string, string> data, object o)
    {
        DataProperty name = prop.GetCustomAttribute<DataProperty>()!;
        string nameStr = name.GetName(prop);
        string value = data[nameStr];

        if (value.Length < Min || value.Length > Max)
            throw new InvalidQueryParameter($"Parameter {nameStr} has an out-of-range or invalid value");

        if (Choices != null)
        {
            if (ChoicesStartsWith && !Choices.Any(value.StartsWith))
                throw new InvalidQueryParameter($"Parameter {nameStr} needs to start with: {string.Join(", ", Choices)}");
            else if (!ChoicesStartsWith && !Choices.Contains(value))
                throw new InvalidQueryParameter($"Parameter {nameStr} needs to be one of the following options: {string.Join(", ", Choices)}");
        }

        prop.SetValue(o, value);
        return true;
    }

    public AsString(int min, int max)
    {
        Min = min;
        Max = max;
    }

    public AsString(int max)
    {
        Max = max;
    }

    public AsString(string[] choices, bool choicesStartsWith = false)
    {
        Choices = choices.ToList();
        ChoicesStartsWith = choicesStartsWith;
    }

    public AsString()
    {
    }
}