using LocoSQS.Model.ActionResults;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocoSQS.Test.Extensions
{
    public static class GetQueueAttributeResponseExtensions
    {
        public static string Attribute(this GetQueueAttributesResponse response, string key)
        {
            return response.Attributes.Find(x => x.Name == key)?.Value ?? throw new Exception($"Key {key} not found");
        }
    }
}
