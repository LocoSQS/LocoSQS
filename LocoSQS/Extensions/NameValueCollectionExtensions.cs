using System.Collections.Specialized;

namespace LocoSQS.Extensions
{
    public static class NameValueCollectionExtensions
    {
        public static Dictionary<string, string> ToDictionary(this NameValueCollection collection)
        {
            Dictionary<string, string> res = new();
            foreach (var x in collection.AllKeys)
            {
                if (x == null)
                    continue;

                res[x] = collection[x]!;
            }

            return res;
        }
    }
}
