using System.Reflection;

namespace LocoSQS.Parser.Attributes;

public class AsObject : Attribute, CustomAttribute
{
    public int Order => 10;
    public bool Apply(PropertyInfo prop, IDictionary<string, string> data, object o)
    {
        string name = prop.GetCustomAttribute<DataProperty>()!.GetName(prop);

        Dictionary<string, string> filtered = new();
        
        foreach (var (key, value) in data)
        {
            if (key.StartsWith(name))
            {
                string[] split = key.Split(".", 2);

                if (split.Length != 2)
                    throw new Exception();
                
                filtered.Add(split[1], value);
                
            }
        }
        
        object result = Activator.CreateInstance(prop.PropertyType)!;
        
        foreach (var propertyInfo in prop.PropertyType.GetProperties())
        {
            foreach (var customAttribute in propertyInfo.GetCustomAttributes().Where(x => x is CustomAttribute).Cast<CustomAttribute>().OrderBy(x => x.Order))
            {
                if (!customAttribute.Apply(propertyInfo, filtered, result))
                    break;
            }
        }
        
        prop.SetValue(o, result);

        return true;
    }
}