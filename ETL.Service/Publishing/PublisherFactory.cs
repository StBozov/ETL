namespace ETL.Service.Publishing
{
    // No need of thread safety for now
    public static class PublisherFactory
    {
        private static readonly IPublisher publisher;

        static PublisherFactory()
        {
            publisher = new KafkaPublisher();
        }

        public static IPublisher Create()
        {
            return publisher;
        }
    }
}