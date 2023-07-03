namespace LocoSQS.Exeptions
{
    public class MissingParameter : SQSException
    {
        public MissingParameter(string message = "A required parameter for the specified action is not supplied") : base(message, 400, "MissingParameter")
        {
        }
    }
}
