namespace LocoSQS.Exeptions;

public class InvalidQueryParameter : SQSException
{
    public InvalidQueryParameter(string message = "The AWS query string is malformed or does not adhere to AWS standards.") : base(message, 400, "InvalidQueryParameter")
    {
    }
}