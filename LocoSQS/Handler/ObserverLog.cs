namespace LocoSQS.Handler
{
    public record ObserverLog(string MessageId, string MessageJson, DateTimeOffset At, string Event);
}
