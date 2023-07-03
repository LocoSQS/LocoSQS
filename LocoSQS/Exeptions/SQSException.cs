using LocoSQS.Model.ActionResults;
using LocoSQS.Model.Utils;

namespace LocoSQS.Exeptions
{
    public class SQSException : Exception
    {
        public int HttpCode { get; }
        public string Code { get; }

        public ActionResult AsResult => new(HttpCode, new ErrorResponse(new()
        {
            Code = Code,
            Type = "Sender",
            Message = Message
        }));

        public SQSException(string message, int httpCode, string code) : base(message)
        {
            Code = code;
            HttpCode = httpCode;
        }
    }
}
