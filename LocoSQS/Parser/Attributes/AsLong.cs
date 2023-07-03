using System.Reflection;

namespace LocoSQS.Parser.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class AsLong : Attribute, CustomAttribute
{
    public int Order => 10;
    public bool Apply(PropertyInfo prop, IDictionary<string, string> data, object o)
    {
        DataProperty name = prop.GetCustomAttribute<DataProperty>()!;
        prop.SetValue(o, long.Parse(data[name.GetName(prop)]));
        return true;
    }
}