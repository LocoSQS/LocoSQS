namespace LocoSQS.Exeptions
{
    public class MissingAction : SQSException
    {

        public MissingAction(string message = "Missing required parameter action or action-specific parameter") : base(message, 400, "MissingAction") { }
    }
}
