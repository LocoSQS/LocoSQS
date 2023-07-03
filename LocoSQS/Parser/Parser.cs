using System.Reflection;
using LocoSQS.Exeptions;
using LocoSQS.Extensions;
using LocoSQS.Model.Interfaces;

namespace LocoSQS.Parser;

public static class Parser
{
    public static Type GetType<T>(string action)
    {
        var types = AppDomain.CurrentDomain.GetAssemblies().SelectMany(x => x.GetTypes())
            .Where(x => x.IsClass && typeof(T).IsAssignableFrom(x)).ToList();

        var type = types.FirstOrDefault(x => x.Name == action);

        if (type == null)
            throw new InvalidAction($"Could not find class with name '{action}'");

        return type;
    }
    
    public static T GetClassInstance<T>(Type t)
    {
        // Todo: Check if t can be cast to T
        
        T value = (T) Activator.CreateInstance(t);
        return value!;
    }
    
    public static T Parse<T>(IDictionary<string, string> values)
    {
        Dictionary<string, string> lowercase = new();
        foreach (var (key, s) in values)
        {
            lowercase.Add(key.ToLower(), s);
        }

        values = lowercase;
        /*
        string @namespace;

        if (typeof(IRootAction).IsAssignableFrom(typeof(T)))
            @namespace = "LocoSQS.Parser.RootActions";
        else if (typeof(IQueueAction).IsAssignableFrom(typeof(T)))
            @namespace = "LocoSQS.Parser.QueueActions";
        else
            throw new NotImplementedException($"Cannot process type {typeof(T).Name}");
        */

        if (!values.ContainsKey("action"))
            throw new MissingAction();

        string action = values["action"];
        
        Type type = GetType<T>(action);
        T value = GetClassInstance<T>(type);
        
        foreach (var propertyInfo in type.GetProperties())
        {
            foreach (var customAttribute in propertyInfo.GetCustomAttributes().Where(x => x is CustomAttribute).Cast<CustomAttribute>().OrderBy(x => x.Order))
            {
                if (!customAttribute.Apply(propertyInfo, values, value))
                    break;
            }
        }

        return value;
    }
}