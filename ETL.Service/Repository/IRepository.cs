namespace ETL.Service.Repository
{
    public interface IRepository
    {
        Task<int> GetRevenue(string userId);
    }
}