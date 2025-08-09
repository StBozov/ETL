using ETL.Service.Auth;
using ETL.Service.Events;
using ETL.Service.Publishing;
using ETL.Service.Repository;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// Use DI if more services are added in the future
IPublisher publisher = PublisherFactory.Create();
IRepository repository = RepositoryFactory.Create();
IAuthProvider authProvider = AuthProviderFactory.Create();

// TODO: Ideally /userRevenue method should be extracted to a separate service
// for better scaling strategy (K8S)
app.MapGet("/userRevenue", async (HttpRequest req, string userId) =>
{
    string token = TokenExtractor.Extract(req);

    if (!await authProvider.IsValidToken(token))
    {
        return Results.Unauthorized();
    }

    // TODO: `userId` must be validated
    int revenue = await repository.GetRevenue(userId);

    return Results.Ok(revenue);
});

app.MapPost("/liveEvent", async (HttpRequest req, LiveEvent e) =>
{
    string token = TokenExtractor.Extract(req);

    if (!await authProvider.IsValidToken(token))
    {
        return Results.Unauthorized();
    }

    // TODO: `e` must be validated
    var isSuccess = await publisher.Publish(e);

    return isSuccess
        ? Results.Ok()
        : Results.Problem();
});

app.Run();