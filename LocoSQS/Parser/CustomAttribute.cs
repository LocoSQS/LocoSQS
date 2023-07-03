using System.Reflection;
using LocoSQS.Parser.Attributes;

namespace LocoSQS.Parser;

public interface CustomAttribute
{
    public int Order { get; }
    bool Apply(PropertyInfo prop, IDictionary<string, string> data, object o);
}