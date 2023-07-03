namespace LocoSQS.Model.ActionResults.Properties
{
    public class ResponseMetadata
    {
        public string RequestId { get; set; } = Guid.NewGuid().ToString();
    }
}
