using LocoSQS.IntegrationTest.Specflow.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocoSQS.IntegrationTest.Specflow.StepDefinitions
{
    [Binding]
    public class PurgeQueueActionDefinitions
    {
        public SharedData SharedData { get; set; }
        public PurgeQueueActionDefinitions(SharedData sharedData)
        {
            SharedData = sharedData;
        }

        [When("i purge the queue")]
        public async Task PurgeQueue()
        {
            await EnvironmentSetup.CLIENT.PurgeQueueAsync(SharedData.QueueUrl);
        }
    }
}
