namespace LocoSQS.Model.ActionResults.Properties
{
    public class BatchResultErrorEntry
    {
        public string Id { get; set; }
        public string Code { get; set; }
        public string Message { get; set; }
        public bool SenderFault { get; set; }

        public BatchResultErrorEntry()
        {
        }

        public BatchResultErrorEntry(string id, string code, string message, bool senderFault)
        {
            Id = id;
            Code = code;
            Message = message;
            SenderFault = senderFault;
        }
    }
}
