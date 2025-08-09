namespace ETL.Client.Events
{
    public interface IEventsProvider
    {
        IAsyncEnumerable<LiveEvent> GetEvents();
    }
}