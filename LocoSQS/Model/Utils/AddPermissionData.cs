namespace LocoSQS.Model.Utils
{
    public class AddPermissionData
    {

        public List<string> ActionName { get; set; }
        public List<string> AwsAccountIds { get; set; }

        public AddPermissionData(List<string> actionName, List<string> AwsAccountIds)
        {
            ActionName = actionName;
            this.AwsAccountIds = AwsAccountIds;
        }

        public AddPermissionData() { }
    }
}
