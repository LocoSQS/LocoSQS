using System.Text;

namespace LocoSQS.Model.Utils
{
    public static class MessageUserAttributesHashCalculator
    {
        public static string Calculate(List<MessageUserAttribute> attributes)
        {
            if (attributes == null)
                throw new ArgumentNullException("attributes");

            if (!BitConverter.IsLittleEndian)
                throw new NotSupportedException("Systems that use little endian are not supported");

            List<byte> stream = new();

            foreach (var attribute in attributes.OrderBy(x => x.Name))
            {
                stream.AddRange(BitConverter.GetBytes(attribute.Name.Length).Reverse());
                stream.AddRange(Encoding.UTF8.GetBytes(attribute.Name));
                stream.AddRange(BitConverter.GetBytes(attribute.DataType.Length).Reverse());
                stream.AddRange(Encoding.UTF8.GetBytes(attribute.DataType));

                if (attribute.StringValue != null)
                {
                    stream.Add(0x1);
                    stream.AddRange(BitConverter.GetBytes(attribute.StringValue.Length).Reverse());
                    stream.AddRange(Encoding.UTF8.GetBytes(attribute.StringValue));
                }
                else if (attribute.BinaryValue != null)
                {
                    stream.Add(0x2);
                    byte[] encodedData = Convert.FromBase64String(attribute.BinaryValue);
                    stream.AddRange(BitConverter.GetBytes(encodedData.Length).Reverse());
                    stream.AddRange(encodedData);
                }
                else throw new Exception("Invalid attribute value"); 
            }

            using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
            {
                byte[] hashBytes = md5.ComputeHash(stream.ToArray());

                string result = Convert.ToHexString(hashBytes).ToLower();
                return result;
            }
        }
    }
}
