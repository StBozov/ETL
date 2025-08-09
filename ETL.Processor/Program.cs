using System.Text.Json;
using Confluent.Kafka;
using ETL.Processor.Events;
using ETL.Processor.Repository;

// TODO: Consider using kafka partitioning (by userId) and launching several instances of ETL.Processor for horizontal scaling
class Program
{
    private static readonly string bootstrapServers = "localhost:9092";
    private static readonly string groupId = "etl-live-events-group";
    private static readonly string topic = "live-events";

    private static readonly ConsumerConfig config;
    private static readonly IRepository repository;

    static Program()
    {
        config = new ConsumerConfig
        {
            BootstrapServers = bootstrapServers,
            GroupId = groupId,
            AutoOffsetReset = AutoOffsetReset.Earliest
        };

        repository = RepositoryFactory.Create();
    }

    static void Main()
    {
        using var consumer = new ConsumerBuilder<Ignore, string>(config).Build();
        consumer.Subscribe(topic);

        Console.WriteLine("Kafka consumer started. Listening for messages...");

        while (true)
        {
            try
            {
                var consumeResult = consumer.Consume();
                Consume(consumeResult);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error occurred: {ex.Message}");
            }
        }

    }

    private static void Consume(ConsumeResult<Ignore, string> consumeResult)
    {
        string liveEventJson = consumeResult.Message.Value;
        var liveEvent = JsonSerializer.Deserialize<LiveEvent>(liveEventJson);

        if (liveEvent is null)
        {
            Console.WriteLine("Failed to deserialize LiveEvent.");
        }
        else
        {
            // TODO: consider processing batch of live events for better performance
            AddRevenue(liveEvent);
            Console.WriteLine($"Consumed message: {consumeResult.Message.Value}");
        }
    }

    private static void AddRevenue(LiveEvent e)
    {
        int revenue = e.EventName == "add_revenue"
            ? e.RevenueValue
            : -e.RevenueValue;

        // TODO: consider async version of this method
        repository.AddRevenue(e.UserId, revenue);
    }
}