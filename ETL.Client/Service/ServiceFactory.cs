namespace ETL.Client.Service
{
    // No need of thread safety for now
    public static class ServiceFactory
    {
        private static readonly RestService restService;

        static ServiceFactory()
        {
            restService = new RestService();
        }

        public static IService Create()
        {
            return restService;
        }
    }
}