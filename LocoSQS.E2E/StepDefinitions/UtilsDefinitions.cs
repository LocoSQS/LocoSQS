using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocoSQS.IntegrationTest.Specflow.StepDefinitions
{
    [Binding]
    public class UtilsDefinitions
    {

        [When("i wait for (.*) seconds?")]
        public void WaitForXSeconds(int seconds)
        {
            Thread.Sleep(seconds * 1000);
        }
    }
}
