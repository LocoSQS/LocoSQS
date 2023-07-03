using Amazon.SQS.Model;
using LocoSQS.IntegrationTest.Specflow.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocoSQS.E2E.StepDefinitions
{
    [Binding]
    public class ChangeMessageVisibilityDefinitions
    {
        public SharedData SharedData { get; set; }

        public ChangeMessageVisibilityDefinitions(SharedData sharedData)
        {
            SharedData = sharedData;
        }

        [When("i make the first message received visible again")]
        public async Task ChangeVisibilityOnFirstMessage()
        {
            await EnvironmentSetup.CLIENT.ChangeMessageVisibilityAsync(SharedData.QueueUrl, SharedData.Messages.First().ReceiptHandle, 0);
        }

        [When("i make all messages received visible again")]
        public async Task ChangeVisibilityOnAllMessages()
        {
            var log = await EnvironmentSetup.CLIENT.ChangeMessageVisibilityBatchAsync(SharedData.QueueUrl, SharedData.Messages.Select(x => new ChangeMessageVisibilityBatchRequestEntry(Guid.NewGuid().ToString(), x.ReceiptHandle) { VisibilityTimeout = 0 }).ToList());
        }
    }
}
