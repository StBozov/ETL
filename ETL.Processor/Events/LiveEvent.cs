namespace ETL.Processor.Events
{
    public record LiveEvent(string UserId, string EventName, int RevenueValue);
}