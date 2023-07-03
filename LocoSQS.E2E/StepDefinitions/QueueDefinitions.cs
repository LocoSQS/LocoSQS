using FluentAssertions;
using LocoSQS.IntegrationTest.Specflow.Support;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow.Infrastructure;

namespace LocoSQS.IntegrationTest.Specflow.StepDefinitions
{
    [Binding]
    public class QueueDefinitions
    {
        public SharedData SharedData { get; set; }
        private ISpecFlowOutputHelper _out;

        public QueueDefinitions(SharedData sharedData, ISpecFlowOutputHelper outputHelper)
        {
            SharedData = sharedData;
            _out = outputHelper;
        }
        [Given("a queue")]
        public async Task SetupQueueWithRandomName() => await SetupQueue(Guid.NewGuid().ToString());

        [Given("queue (.*)")]
        public async Task SetupQueue(string queueName)
        {
            string queueUrl;

            try
            {
                queueUrl = (await EnvironmentSetup.CLIENT.GetQueueUrlAsync(queueName)).QueueUrl;
            }
            catch (Exception e)
            {
                await EnvironmentSetup.CLIENT.CreateQueueAsync(queueName);
                queueUrl = (await EnvironmentSetup.CLIENT.GetQueueUrlAsync(queueName)).QueueUrl;
            }

            SharedData.QueueUrl = queueUrl;
            _out.WriteLine($"Set up Queue: {queueUrl}");
        }

        [Given("the queue has an attribute called (.*) with value (.*)")]
        public async Task SetAttributeOnQueue(string attributeName, string attributeValue)
        {
            await EnvironmentSetup.CLIENT.SetQueueAttributesAsync(SharedData.QueueUrl, new() { { attributeName, attributeValue } });
        }

        [Then("a queue exists")]
        public async Task QueueExistsWithName()
        {
            Assert.That((await EnvironmentSetup.CLIENT.ListQueuesAsync("")).QueueUrls, Has.Member(SharedData.QueueUrl));
        }

        [Then("there are ([0-9]*) messages on the queue")]
        public async Task GetApproxMessageCount(int count)
        {
            var result = await EnvironmentSetup.CLIENT.GetQueueAttributesAsync(SharedData.QueueUrl, new() { "ApproximateNumberOfMessages" });
            result.ApproximateNumberOfMessages.Should().Be(count);
        }

        [Then("there are less than(.*) messages on the queue")]
        public async Task GetLessThanApproxMessageCount(int count)
        {
            var result = await EnvironmentSetup.CLIENT.GetQueueAttributesAsync(SharedData.QueueUrl, new() { "ApproximateNumberOfMessages" });
            result.ApproximateNumberOfMessages.Should().BeLessThan(count);
        }

        [AfterScenario]
        public async Task TeardownQueue()
        {
            try
            {
                if (!string.IsNullOrEmpty(SharedData.QueueUrl))
                    await EnvironmentSetup.CLIENT.DeleteQueueAsync(SharedData.QueueUrl);

                SharedData.QueueUrl = "";
            }
            catch { }
        }
    }
}
