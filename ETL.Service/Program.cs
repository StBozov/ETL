using ETL.Service.Auth;
using ETL.Service.Events;
using ETL.Service.Publishing;

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

IAuthProvider authProvider = AuthProviderFactory.Create();
IPublisher publisher = PublisherFactory.Create();

app.MapGet("/userRevenue", async (HttpRequest req, string userId) =>
{
    // TODO: `userId` must be validated in a real life scenario
    string token = TokenExtractor.Extract(req);

    if (!await authProvider.IsValidToken(token))
    {
        return Results.Unauthorized();
    }

    // TODO: Implement logic to retrieve user revenue from the repository
    return Results.Ok();
});

app.MapPost("/liveEvent", async (HttpRequest req, LiveEvent e) =>
{
    // TODO: `e` must be validated in a real life scenario
    string token = TokenExtractor.Extract(req);

    if (!await authProvider.IsValidToken(token))
    {
        return Results.Unauthorized();
    }

    var isSuccess = await publisher.Publish(e);

    return isSuccess
        ? Results.Ok()
        : Results.Problem();
});

app.Run();