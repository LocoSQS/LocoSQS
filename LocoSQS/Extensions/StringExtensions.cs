namespace LocoSQS.Extensions;

public static class StringExtensions
{
    public static string AsMD5Hash(this string input)
    {
        // Use input string to calculate MD5 hash
        using (System.Security.Cryptography.MD5 md5 = System.Security.Cryptography.MD5.Create())
        {
            byte[] inputBytes = System.Text.Encoding.ASCII.GetBytes(input);
            byte[] hashBytes = md5.ComputeHash(inputBytes);

            string result = Convert.ToHexString(hashBytes).ToLower();
            return result;
        }
    }
}