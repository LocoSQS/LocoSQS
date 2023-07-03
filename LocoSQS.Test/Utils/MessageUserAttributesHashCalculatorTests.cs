using LocoSQS.Model.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocoSQS.Test.Utils
{
    internal class MessageUserAttributesHashCalculatorTests
    {
        [Test]
        public void TestStringHash()
        {
            MessageUserAttribute attribute = new()
            {
                Name = "test",
                StringValue = "test",
                DataType = "String"
            };

            string result = MessageUserAttributesHashCalculator.Calculate(new() { attribute });
            Assert.That(result, Is.EqualTo("ddb45ae313fa7f1b0fbf07d6f3b9e1c5"));
        }

        [Test]
        public void TestNumberHash()
        {
            MessageUserAttribute attribute = new()
            {
                Name = "test2",
                StringValue = "1",
                DataType = "Number"
            };

            string result = MessageUserAttributesHashCalculator.Calculate(new() { attribute });
            Assert.That(result, Is.EqualTo("142cc537795243d15d2cd4e2923d9646"));
        }

        [Test]
        public void TestBinaryHash()
        {
            MessageUserAttribute attribute = new()
            {
                Name = "test3",
                BinaryValue = "c3Vw",
                DataType = "Binary"
            };

            string result = MessageUserAttributesHashCalculator.Calculate(new() { attribute });
            Assert.That(result, Is.EqualTo("d9f7e8a54879aa34ee433a40a252d417"));
        }

        [Test]
        public void TestHashOrder()
        {
            MessageUserAttribute test1 = new()
            {
                Name = "test1",
                StringValue = "test",
                DataType = "String"
            };

            MessageUserAttribute test2 = new()
            {
                Name = "test2",
                StringValue = "1",
                DataType = "Number"
            };

            MessageUserAttribute test3 = new()
            {
                Name = "test3",
                BinaryValue = "c3Vw",
                DataType = "Binary"
            };

            string result = MessageUserAttributesHashCalculator.Calculate(new() { test2, test1, test3 });
            Assert.That(result, Is.EqualTo("38b8bf1a666b57dc78184e487d8290f1"));
        }
    }
}
