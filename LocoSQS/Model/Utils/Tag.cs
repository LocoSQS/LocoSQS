using LocoSQS.Parser.Attributes;

namespace LocoSQS.Model.Utils
{
    public class Tag
    {
        [DataProperty]
        [AsString]
        public string Key { get; set; } = "";

        [DataProperty]
        [AsString]
        public string Value { get; set; } = "";
    }
}
