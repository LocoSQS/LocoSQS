namespace LocoSQS.Exeptions
{
    public class InvalidAction : SQSException
    {
        public InvalidAction(string message = "Specified Action does not exist") : base(message, 400, "InvalidAction") { }
    }
}
