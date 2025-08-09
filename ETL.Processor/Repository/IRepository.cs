namespace ETL.Processor.Repository
{
    public interface IRepository
    {
        // TODO: consider async version of this method
        void AddRevenue(string userId, int revenue);
    }
}