namespace LocoSQS.Extensions
{
    public static class HttpContextExtensions
    {
        public static async Task<string> ReadBodyAsString(this Microsoft.AspNetCore.Http.HttpContext ctx)
        {
            using (StreamReader reader = new(ctx.Request.Body))
            {
                return await reader.ReadToEndAsync();
            }
        }
    }
}
