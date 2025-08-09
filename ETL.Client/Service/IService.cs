using ETL.Client.Events;

namespace ETL.Client.Service
{
    public interface IService
    {
        Task<UserRevenue> GetUserRevenue(string userId, string token);
        Task<bool> PostLiveEvent(LiveEvent e, string token);
    }
}