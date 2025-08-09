using ETL.Client.Events;

namespace ETL.Client.Service
{
    public interface IService
    {
        Task<UserRevenue> GetUserRevenue(string userId);
        Task<bool> PostLiveEvent(LiveEvent e);
    }
}