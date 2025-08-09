namespace ETL.Client.Service
{
    // No need of thread safety for now
    public static class ServiceFactory
    {
        private static RestService? restService;

        public static IService Create(string token)
        {
            return restService ??= new RestService(token);
        }
    }
}