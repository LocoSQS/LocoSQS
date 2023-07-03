using LocoSQS.Model.Utils;
using LocoSQS.Test.Extensions;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace LocoSQS.Test.StandardQueue
{
    internal class StandardQueueTests
    {
        #region Serilization Tests
        [Test]
        public void SerializeDelaySeconds()
        {
            for (int i = 0; i <= 900; i++)
            {
                Queue.Standard.StandardQueue standardQueue = new(new()
                {
                    Attributes =
                {
                    {"DelaySeconds", i.ToString()}
                }
                }, null);

                Assert.That(standardQueue.DelaySeconds, Is.EqualTo(TimeSpan.FromSeconds(i)));
            }

            try
            {
                Queue.Standard.StandardQueue standardQueue = new(new()
                {
                    Attributes =
                {
                    {"DelaySeconds", "1000"}
                }
                }, null);
                throw new Exception("No exception was thrown");
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Is.EqualTo("DelaySeconds property can only be between 0 and 900"));
            }
        }

        [Test]
        public void SerializeMaximumMessageSize()
        {
            for (int i = 0x400; i <= 0x40000; i++)
            {
                Queue.Standard.StandardQueue standardQueue = new(new()
                {
                    Attributes =
                {
                    {"MaximumMessageSize", i.ToString()}
                }
                }, null);

                Assert.That(standardQueue.MaximumMessageSize, Is.EqualTo(i));
            }

            try
            {
                Queue.Standard.StandardQueue standardQueue = new(new()
                {
                    Attributes =
                {
                    {"MaximumMessageSize", "0"}
                }
                }, null);
                throw new Exception("No exception was thrown");
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Is.EqualTo($"MaximumMessageSize property can only be between {0x400} and {0x40000}"));
            }
        }

        [Test]
        public void SerializeMessageRetentionPeriod()
        {
            for (int i = 60; i <= 1209600; i += 10)
            {
                Queue.Standard.StandardQueue standardQueue = new(new()
                {
                    Attributes =
                {
                    {"MessageRetentionPeriod", i.ToString()}
                }
                }, null);

                Assert.That(standardQueue.MessageRetentionPeriod, Is.EqualTo(TimeSpan.FromSeconds(i)));
            }

            try
            {
                Queue.Standard.StandardQueue standardQueue = new(new()
                {
                    Attributes =
                {
                    {"MessageRetentionPeriod", "1209601"}
                }
                }, null);
                throw new Exception("No exception was thrown");
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Is.EqualTo($"MessageRetentionPeriod property can only be between 60 and 1209600"));
            }
        }

        [Test]
        public void SerializePolicy()
        {
            Queue.Standard.StandardQueue standardQueue = new(new()
            {
                Attributes =
                {
                    {"Policy", "Hello"}
                }
            }, null);

            Assert.That(standardQueue.Policy, Is.EqualTo("Hello"));
        }

        [Test]
        public void SerializeReceiveMessageWaitTimeSeconds()
        {
            for (int i = 0; i <= 20; i++)
            {
                Queue.Standard.StandardQueue standardQueue = new(new()
                {
                    Attributes =
                {
                    {"ReceiveMessageWaitTimeSeconds", i.ToString()}
                }
                }, null);

                Assert.That(standardQueue.ReceiveMessageWaitTimeSeconds, Is.EqualTo(TimeSpan.FromSeconds(i)));
            }

            try
            {
                Queue.Standard.StandardQueue standardQueue = new(new()
                {
                    Attributes =
                {
                    {"ReceiveMessageWaitTimeSeconds", "1209601"}
                }
                }, null);
                throw new Exception("No exception was thrown");
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Is.EqualTo($"ReceiveMessageWaitTimeSeconds property can only be between 0 and 20"));
            }
        }

        [Test]
        public void SerializeVisibilityTimeout()
        {
            for (int i = 0; i <= 43200; i++)
            {
                Queue.Standard.StandardQueue standardQueue = new(new()
                {
                    Attributes =
                {
                    {"VisibilityTimeout", i.ToString()}
                }
                }, null);

                Assert.That(standardQueue.VisibilityTimeout, Is.EqualTo(TimeSpan.FromSeconds(i)));
            }

            try
            {
                Queue.Standard.StandardQueue standardQueue = new(new()
                {
                    Attributes =
                {
                    {"VisibilityTimeout", "1209601"}
                }
                }, null);
                throw new Exception("No exception was thrown");
            }
            catch (Exception e)
            {
                Assert.That(e.Message, Is.EqualTo($"VisibilityTimeout property can only be between 0 and 43200"));
            }
        }

        [Test]
        public void SerializeRedrivePolicy()
        {
            RedrivePolicy policy = new()
            {
                DeadLetterTargetArn = "Test",
                MaxReceiveCount = Random.Shared.Next(0, 10000)
            };

            Queue.Standard.StandardQueue standardQueue = new(new()
            {
                Attributes =
                {
                    {"RedrivePolicy", JsonConvert.SerializeObject(policy)}
                }
            }, null);

            Assert.That(standardQueue.RedrivePolicy.DeadLetterTargetArn, Is.EqualTo(policy.DeadLetterTargetArn));
            Assert.That(standardQueue.RedrivePolicy.MaxReceiveCount, Is.EqualTo(policy.MaxReceiveCount));
        }
        #endregion

        [Test]
        public void GetQueueAttributesDefault()
        {
            Queue.Standard.StandardQueue standardQueue = new(new(), null);

            var res = standardQueue.GetQueueAttributes(new() { "All" });
            Assert.That(res.Value.Attribute("DelaySeconds"), Is.EqualTo("0"));
            Assert.That(res.Value.Attribute("MaximumMessageSize"), Is.EqualTo("262144"));
            Assert.That(res.Value.Attribute("MessageRetentionPeriod"), Is.EqualTo("345600"));
            Assert.That(res.Value.Attribute("Policy"), Is.EqualTo("{}"));
            Assert.That(res.Value.Attribute("ReceiveMessageWaitTimeSeconds"), Is.EqualTo("0"));
            Assert.That(res.Value.Attribute("VisibilityTimeout"), Is.EqualTo("30"));
            
            try
            {
                res.Value.Attribute("RedrivePolicy");
                throw new Exception("RedrivePolicy Exists");
            }
            catch(Exception e)
            {
                Assert.That(e.Message, Is.EqualTo("Key RedrivePolicy not found"));
            }
        }

        [Test]
        public void GetQueueAttributesCustom()
        {
            Queue.Standard.StandardQueue standardQueue = new(new(), null);
            standardQueue.CreatedTimestamp = new DateTimeOffset(2023, 4, 5, 0, 0, 0, TimeSpan.Zero);
            standardQueue.DelaySeconds = TimeSpan.FromHours(1);
            standardQueue.LastModifiedTimestamp = new DateTimeOffset(2023, 4, 6, 0, 0, 0, TimeSpan.Zero);
            standardQueue.MaximumMessageSize = 2;
            standardQueue.Policy = "3";
            standardQueue.ReceiveMessageWaitTimeSeconds = TimeSpan.FromHours(4);
            standardQueue.VisibilityTimeout = TimeSpan.FromHours(5);
            standardQueue.RedrivePolicy = new()
            {
                DeadLetterTargetArn = "6",
                MaxReceiveCount = 7
            };

            var result = standardQueue.GetQueueAttributes(new() { "All" });

            Assert.That(result.Value.Attribute("CreatedTimestamp"), Is.EqualTo("1680652800"));
            Assert.That(result.Value.Attribute("LastModifiedTimestamp"), Is.EqualTo("1680739200"));
            Assert.That(result.Value.Attribute("DelaySeconds"), Is.EqualTo("3600"));
            Assert.That(result.Value.Attribute("MaximumMessageSize"), Is.EqualTo("2"));
            Assert.That(result.Value.Attribute("Policy"), Is.EqualTo("3"));
            Assert.That(result.Value.Attribute("ReceiveMessageWaitTimeSeconds"), Is.EqualTo("14400"));
            Assert.That(result.Value.Attribute("VisibilityTimeout"), Is.EqualTo("18000"));
            Assert.That(result.Value.Attribute("RedrivePolicy"), Is.EqualTo("{\"deadLetterTargetArn\":\"6\",\"maxReceiveCount\":7}"));
        }
    }
}
