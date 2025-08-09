using System.Text.Json;
using Confluent.Kafka;
using ETL.Processor.Events;
using ETL.Processor.Repository;

// TODO: Consider using kafka partitioning (by userId) and launching several instances of ETL.Processor for horizontal scaling
class Program
{
    // TODO: bootstrapServers, groupId, topic must be get by some kind of config (local or remote)
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

        try
        {
            while (true)
            {
                var consumeResult = consumer.Consume();

                // TODO: Consider advanced error handling if the Consume invocation failed. For the time being just exit
                // so that we do not end up with inconsistent state
                Consume(consumeResult);
            }

        }
        catch (Exception ex)
        {
            Console.WriteLine($"Error occurred: {ex.Message}");
        }
    }

    private static void Consume(ConsumeResult<Ignore, string> consumeResult)
    {
        string liveEventJson = consumeResult.Message.Value;
        var liveEvent = JsonSerializer.Deserialize<LiveEvent>(liveEventJson);

        // TODO: liveEvent must be validated before processing
        if (liveEvent is null)
        {
            Console.WriteLine("Failed to deserialize LiveEvent.");
        }
        else
        {
            AddRevenue(liveEvent);
            Console.WriteLine($"Consumed message: {consumeResult.Message.Value}");
        }
    }

    private static void AddRevenue(LiveEvent e)
    {
        // TODO: if too many invocations of repository.AddRevenue are made then 
        // a performance issue may arise (regarding the DB), 
        // so consider events batching (calculation) before invoking repository.AddRevenue

        int revenue = e.EventName == "add_revenue"
            ? e.RevenueValue
            : -e.RevenueValue;

        repository.AddRevenue(e.UserId, revenue);
    }
}