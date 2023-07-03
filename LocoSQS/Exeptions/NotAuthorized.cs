namespace LocoSQS.Exeptions;

public class NotAuthorized : SQSException
{
    public NotAuthorized(string message = "You do not have permission to perform this action.") : base(message, 400, "NotAuthorized")
    {
    }
}