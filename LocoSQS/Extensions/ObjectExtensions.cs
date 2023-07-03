namespace LocoSQS.Extensions;

public static class ObjectExtensions
{
    public static T Require<T>(this T? o, string message = "Missing value")
    {
        if (o == null)
            throw new Exception(message);

        return o;
    }
}