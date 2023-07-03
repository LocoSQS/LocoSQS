using LocoSQS.Exeptions;
using System.Reflection;

namespace LocoSQS.Parser.Attributes;

[AttributeUsage(AttributeTargets.Property, Inherited = false)]
public class AsList : Attribute, CustomAttribute
{
    public int Max { get; set; } = int.MaxValue;
    public int Order => 10;
    public bool Apply(PropertyInfo prop, IDictionary<string, string> data, object o)
    {
        string name = prop.GetCustomAttribute<DataProperty>()!.GetName(prop);

        Dictionary<string, Dictionary<string, string>> splitData = new();
        
        foreach (var (key, s) in data)
        {
            if (key.StartsWith(name))
            {
                string[] split = key.Split(".", 3);

                if (!splitData.ContainsKey(split[1]))
                    splitData[split[1]] = new();

                if (split.Length <= 2)
                    splitData[split[1]]["_"] = s;
                else
                    splitData[split[1]][split[2]] = s;
            }
        }

        if (splitData.Count > Max)
            throw new InvalidQueryParameter($"Parameter {name} can only have a max of {Max} list elements");
        
        List<Dictionary<string, string>> sortedSplitData = 
            splitData.OrderBy(x => x.Key).Select(x => x.Value).ToList();
        
        Type listType = prop.PropertyType.GetGenericArguments().Single();
        var valueType = typeof(List<>);
        object value = Activator.CreateInstance(valueType.MakeGenericType(listType))!;
        
        foreach (var dictionary in sortedSplitData)
        {
            // TODO: Currently assumes list is of type List<string>
            if (dictionary.ContainsKey("_"))
            {
                value.GetType().GetMethod("Add")!.Invoke(value, new[] {dictionary["_"]});
            }
            else
            {
                object innerValue = Activator.CreateInstance(listType)!;
                
                foreach (var propertyInfo in listType.GetProperties())
                {
                    foreach (var customAttribute in propertyInfo.GetCustomAttributes().Where(x => x is CustomAttribute).Cast<CustomAttribute>().OrderBy(x => x.Order))
                    {
                        if (!customAttribute.Apply(propertyInfo, dictionary, innerValue))
                            break;
                    }
                }
                
                value.GetType().GetMethod("Add")!.Invoke(value, new[] {innerValue});
            }
        }
        
        prop.SetValue(o, value);
        return true;
    }

    public AsList()
    {
    }

    public AsList(int max)
    {
        Max = max;
    }
}