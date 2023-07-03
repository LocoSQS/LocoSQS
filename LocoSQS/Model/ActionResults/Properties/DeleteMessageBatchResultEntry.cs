namespace LocoSQS.Model.ActionResults.Properties
{
    public class DeleteMessageBatchResultEntry
    {
        public string Id { get; set; }

        public DeleteMessageBatchResultEntry(string id)
        {
            Id = id;
        }

        public DeleteMessageBatchResultEntry()
        {
        }
    }
}
