using ETL.Client.Auth;
using ETL.Client.Events;
using ETL.Client.Service;

class Program
{
    static async Task Main()
    {
        IService service = CreateService();

        // TODO: `token` must be refreshed in a real life scenario
        string token = await GetToken();

        await foreach (LiveEvent e in GetEvents())
        {
            // TODO: `e` must be validated in a real life scenario
            Console.WriteLine($"posting live event: {e}");

            bool success = await service.PostLiveEvent(e, token);
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

    private static IService CreateService() => ServiceFactory.Create();
}