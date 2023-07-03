namespace LocoSQS.Model.ActionResults.Properties
{
    public class ChangeMessageVisibilityBatchResultEntry
    {
        public string Id { get; set; }

        public ChangeMessageVisibilityBatchResultEntry(string id)
        {
            Id = id;
        }

        public ChangeMessageVisibilityBatchResultEntry()
        {
        }
    }
}
