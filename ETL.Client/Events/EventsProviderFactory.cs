namespace ETL.Client.Events
{
    // No need of thread safety for now
    public static class EventsProviderFactory
    {
        private static readonly IEventsProvider eventsProvider;

        static EventsProviderFactory()
        {
            eventsProvider = new LocalFileEventsProvider();
        }

        public static IEventsProvider Create()
        {
            return eventsProvider;
        }
    }
}