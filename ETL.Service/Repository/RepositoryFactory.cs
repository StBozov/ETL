namespace ETL.Service.Repository
{
    // No need of thread safety for now
    public static class RepositoryFactory
    {
        private static readonly IRepository repository;
        static RepositoryFactory()
        {
            repository = new PostgreRepository();
        }

        public static IRepository Create()
        {
            return repository;
        }
    }
}