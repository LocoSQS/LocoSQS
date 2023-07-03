namespace LocoSQS.Exeptions
{
    public class ValidationError : SQSException
    {

        public ValidationError(string message) : base(message, 400, "ValidationError") { }
    }
}
