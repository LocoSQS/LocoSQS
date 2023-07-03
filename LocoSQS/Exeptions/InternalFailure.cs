namespace LocoSQS.Exeptions
{
    public class InternalFailure : SQSException
    {
        public InternalFailure(string message) : base(message, 500, "InternalFailure")
        {
        }
    }
}
