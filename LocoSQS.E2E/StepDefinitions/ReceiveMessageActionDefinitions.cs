using Amazon.SQS.Model;
using LocoSQS.IntegrationTest.Specflow.Support;
using NUnit.Framework;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocoSQS.IntegrationTest.Specflow.StepDefinitions
{
    [Binding]
    public class ReceiveMessageActionDefinitions
    {
        public SharedData SharedData { get; set; }
        

        public ReceiveMessageActionDefinitions(SharedData sharedData)
        {
            SharedData = sharedData;
        }

        [When("i ask for (.*) messages?")]
        public async Task GetMessagesNormal(int count) => await GetMessages(count, false, false);

        [When("i ask for (.*) messages? silently")]
        public async Task GetMessagesSilently(int count) => await GetMessages(count, true, false);

        [When("i ask for (.*) messages? with attributes")]
        public async Task GetMessagesNormalAttr(int count) => await GetMessages(count, false, true);

        [When("i ask for (.*) messages? silently with attributes")]
        public async Task GetMessagesSilentlyAttr(int count) => await GetMessages(count, true, true);

        private async Task GetMessages(int count, bool silent, bool withAttributes)
        {
            ReceiveMessageRequest request = new(SharedData.QueueUrl)
            {
                WaitTimeSeconds = 1,
                MaxNumberOfMessages = count,
                MessageAttributeNames = (withAttributes) ? new() { "All" } : new() { },
                AttributeNames = (withAttributes) ? new() { "All" } : new() { },
            };

            if (silent)
                request.VisibilityTimeout = 0;
            
            var response = await EnvironmentSetup.CLIENT.ReceiveMessageAsync(request);
            SharedData.Messages = response.Messages;
        }

        [When("i exhaust the queue by constantly reading messages")]
        public async Task GetMessagesContinously() => await GetMessagesContinously(0);

        [When("i exhaust the queue by constantly reading messages for at least (.*) seconds")]
        public async Task GetMessagesContinously(int minTime)
        {
            ReceiveMessageRequest request = new(SharedData.QueueUrl)
            {
                WaitTimeSeconds = 3,
            };

            SharedData.Messages = new();
            Stopwatch minWatch = Stopwatch.StartNew();
            Stopwatch stopwatch = Stopwatch.StartNew();
            while (stopwatch.Elapsed < TimeSpan.FromSeconds(2.5) || minWatch.Elapsed < TimeSpan.FromSeconds(minTime))
            {
                stopwatch.Restart();
                var response = await EnvironmentSetup.CLIENT.ReceiveMessageAsync(request);
                SharedData.Messages.AddRange(response.Messages);
            }
        }

        [Then("i g[e|o]t (.*) messages?")]
        public void CountMessages(int count)
        {
            Assert.That(SharedData.Messages.Count, Is.EqualTo(count));
        }

        [Then("any message has an attribute called (.*)")]
        public void HasAttribute(string attributeName)
        {
            Assert.That(SharedData.Messages.Any(x => x.Attributes.ContainsKey(attributeName)), Is.EqualTo(true));
        }

        [Then("no message has an attribute called (.*)")]
        public void NoAttribute(string attributeName)
        {
            Assert.That(SharedData.Messages.All(x => !x.Attributes.ContainsKey(attributeName)), Is.EqualTo(true));
        }

        [Then("any message has an attribute with value (.*) called (.*)")]
        public void HasAttributeWithValue(string attributeValue, string attributeName)
        {
            Message? m = SharedData.Messages.Find(x => x.Attributes.ContainsKey(attributeName));

            if (m == null)
                throw new Exception($"Could not find message with attribute {attributeName}");

            Assert.That(m.Attributes[attributeName], Is.EqualTo(attributeValue));
        }

        [Then("any message has (.*) as text")]
        public void HasText(string content)
        {
            Assert.That(SharedData.Messages.Any(x => x.Body == content), Is.EqualTo(true));
        }

        [Then("any message has an user defined attribute called (.*) of type (.*) with value (.*)")]
        public void HasUserDefinedAttribute(string name, string type, string value)
        {
            Message? m = SharedData.Messages.Find(x => x.MessageAttributes.ContainsKey(name));
            
            if (m == null)
                throw new Exception($"Could not find message with user defined attribute {name}");
            
            Assert.That(m.MessageAttributes[name].DataType, Is.EqualTo(type));
            Assert.That(m.MessageAttributes[name].StringValue, Is.EqualTo(value));
        }
        
        [Then("no messages have an user defined attribute called (.*)")]
        public void HasUserDefinedAttribute(string name)
        {
            Message? m = SharedData.Messages.Find(x => x.MessageAttributes.ContainsKey(name));
            
            if (m != null)
                throw new Exception($"Found a message with user defined attribute {name}");
        }
    }
}
