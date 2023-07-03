namespace LocoSQS.Exeptions;

public class InvalidParameterValue : SQSException
{
    public InvalidParameterValue(string message = "An invalid or out-of-range value was supplied for the input parameter.") : base(message, 400, "InvalidParameterValue")
    {
    }
}