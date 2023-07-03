using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.Utils
{
    public class QueueAttribute 
    {
        [DataProperty]
        [AsString]
        public string Name { get; set; } = "";

        [DataProperty]
        [AsString]
        public string Value { get; set; } = "";
    }
}
