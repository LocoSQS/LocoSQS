using LocoSQS.IntegrationTest.Specflow.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocoSQS.IntegrationTest.Specflow.StepDefinitions
{
    [Binding]
    public class DeleteMessageActionDefinitions
    {
        public SharedData SharedData { get; set; }

        public DeleteMessageActionDefinitions(SharedData sharedData)
        {
            SharedData = sharedData;
        }

        [When("i delete the first received message")]
        public async Task DeleteOneMessage()
        {
            await EnvironmentSetup.CLIENT.DeleteMessageAsync(SharedData.QueueUrl, SharedData.Messages.First().ReceiptHandle);
        }

        [When("i delete all received messages")]
        public async Task DeleteMessages()
        {
            await EnvironmentSetup.CLIENT.DeleteMessageBatchAsync(SharedData.QueueUrl, SharedData.Messages.Select(x => new Amazon.SQS.Model.DeleteMessageBatchRequestEntry(Guid.NewGuid().ToString(), x.ReceiptHandle)).ToList());
        }
    }
}
