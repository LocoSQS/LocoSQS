using System.Reflection;
using LocoSQS.Exeptions;

namespace LocoSQS.Parser.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class AsInt : Attribute, CustomAttribute
{
    public int? Min { get; set; } = null;
    public int? Max { get; set; } = null;
    
    public int Order => 10;
    public bool Apply(PropertyInfo prop, IDictionary<string, string> data, object o)
    {
        DataProperty dataProp = prop.GetCustomAttribute<DataProperty>()!;
        string name = dataProp.GetName(prop);
        int x = int.Parse(data[name]);

        if ((Min.HasValue && x < Min) || (Max.HasValue && x > Max))
            throw new InvalidQueryParameter($"Parameter {name} has an out-of-range or invalid value");
        
        prop.SetValue(o, x);
        return true;
    }

    public AsInt()
    {
    }

    public AsInt(int min, int max)
    {
        Min = min;
        Max = max;
    }
}