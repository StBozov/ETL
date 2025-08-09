using ETL.Service.Events;

namespace ETL.Service.Publishing
{
    public interface IPublisher
    {
        Task<bool> Publish(LiveEvent e);
    }
}