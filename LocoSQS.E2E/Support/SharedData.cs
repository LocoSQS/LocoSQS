using Amazon.SQS.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocoSQS.IntegrationTest.Specflow.Support
{
    public class SharedData
    {
        public string QueueUrl { get; set; } = "";
        public List<Message> Messages { get; set; } = new();
    }
}
