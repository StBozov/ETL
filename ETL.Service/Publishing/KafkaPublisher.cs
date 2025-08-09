using System.Text.Json;
using Confluent.Kafka;
using ETL.Service.Events;

namespace ETL.Service.Publishing
{
    public class KafkaPublisher : IPublisher
    {
        private const string Topic = "live-events";

        private readonly IProducer<string, string> producer;

        public KafkaPublisher()
        {
            var kafkaConfig = new ProducerConfig
            {
                BootstrapServers = "localhost:9092"
            };

            producer = new ProducerBuilder<string, string>(kafkaConfig).Build();
        }

        public async Task<bool> Publish(LiveEvent e)
        {
            Message<string, string> message = CreateMessage(e);

            var deliveryResult = await producer.ProduceAsync(Topic, message);

            return deliveryResult.Status == PersistenceStatus.Persisted;
        }

        private Message<string, string> CreateMessage(LiveEvent e)
        {
            string key = e.UserId;
            string value = JsonSerializer.Serialize(e);
            return new Message<string, string> { Key = key, Value = value };
        }
    }
}