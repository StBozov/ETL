using ETL.Client.Auth;
using ETL.Client.Events;
using ETL.Client.Service;

class Program
{
    static async Task Main()
    {
        // TODO: `token` must be refreshed
        string token = await GetToken();

        // TODO: a new service instance must be created for each new token (reusing service instance won't be safe anymore)
        IService service = CreateService(token);

        await foreach (LiveEvent e in GetEvents())
        {
            // TODO: `e` must be validated
            Console.WriteLine($"posting live event: {e}");

            bool success = await service.PostLiveEvent(e);
            Console.WriteLine($"post live event result: {success}");
        }

        Console.WriteLine("All live events posted...");
        Console.ReadLine();
    }

    private static async Task<string> GetToken()
    {
        IAuthProvider authProvider = AuthProviderFactory.Create();

        string? token = await authProvider.GetToken();
        Console.WriteLine($"auth token: {token ?? "<null>"}");

        InvalidDataException exception = new("Invalid authentication token.");
        return token ?? throw exception;
    }

    private static async IAsyncEnumerable<LiveEvent> GetEvents()
    {
        IEventsProvider eventsProvider = EventsProviderFactory.Create();

        // prefer readability over performance
        await foreach (var liveEvent in eventsProvider.GetEvents())
        {
            yield return liveEvent;
        }
    }

    private static IService CreateService(string token) => ServiceFactory.Create(token);
}