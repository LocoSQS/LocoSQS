using LocoSQS.IntegrationTest.Specflow.Support;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Amazon.SQS.Model;

namespace LocoSQS.IntegrationTest.Specflow.StepDefinitions
{
    [Binding]
    public class SendMessageActionDefinitions
    {
        public SharedData SharedData { get; set; }

        public SendMessageActionDefinitions(SharedData sharedData)
        {
            SharedData = sharedData;
        }

        [Given("there is a message on the queue")]
        public async Task SendMessage() => await SendMessage(1);

        [Given("there are (.*) messages on the queue")]
        public async Task SendMessage(int count)
        {
            await EnvironmentSetup.CLIENT.SendMessageBatchAsync(SharedData.QueueUrl, Enumerable.Range(0, count)
                .Select(x => new Amazon.SQS.Model.SendMessageBatchRequestEntry(x.ToString(), x.ToString())).ToList());
        }

        [Given("there is a message with text (.*) on the queue")]
        public async Task SendMessage(string content)
        {
            await EnvironmentSetup.CLIENT.SendMessageAsync(SharedData.QueueUrl, content);
        }

        [Given("there is a message with a user defined attribute called (.*) with type (.*) and value (.*) on the queue")]
        public async Task SendMessageWithUserAttribute(string name, string type, string value)
        {
            await EnvironmentSetup.CLIENT.SendMessageAsync(
                new SendMessageRequest(SharedData.QueueUrl, Guid.NewGuid().ToString())
                {
                    MessageAttributes = new()
                    {
                        {
                            name, new()
                            {
                                StringValue = value,
                                DataType = type
                            }
                        }
                    }
                });
        }
    }
}
