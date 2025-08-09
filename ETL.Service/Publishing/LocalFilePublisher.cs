using ETL.Service.Events;

namespace ETL.Service.Publishing
{
    public class LocalFilePublisher : IPublisher
    {
        public Task<bool> Publish(LiveEvent e)
        {
            throw new NotImplementedException("LocalFilePublisher is not implemented.");
        }
    }
}