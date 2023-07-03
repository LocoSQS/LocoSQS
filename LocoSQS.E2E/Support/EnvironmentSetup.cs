using Amazon.Runtime;
using Amazon.SQS;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TechTalk.SpecFlow;

namespace LocoSQS.IntegrationTest.Specflow.Support
{
    [Binding]
    public static class EnvironmentSetup
    {
        public static AmazonSQSClient CLIENT { get; private set; }

        [BeforeTestRun]
        public static async Task BeforeTestRun()
        {
            string AccessKey = Environment.GetEnvironmentVariable("ACCESSKEY") ?? throw new Exception("No ACCESSKEY environment variable set");
            string SecretKey = Environment.GetEnvironmentVariable("SECRETKEY") ?? throw new Exception("No SECRETKEY environment variable set");
            string ServiceUrl = Environment.GetEnvironmentVariable("SERVICEURL") ?? throw new Exception("No SERVICEURL environment variable set");

            AmazonSQSConfig config = new()
            {
                ServiceURL = ServiceUrl,
                UseHttp = !ServiceUrl.StartsWith("https")
            };

            CLIENT = new AmazonSQSClient(new BasicAWSCredentials(AccessKey, SecretKey), config);
        }
    }
}
