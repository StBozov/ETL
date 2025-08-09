var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

string token = "fake-secret-token";

app.MapPost("/auth", async (HttpRequest req) =>
{
    using var reader = new StreamReader(req.Body);
    var body = await reader.ReadToEndAsync();

    if (body.Contains("user") && body.Contains("password"))
        return Results.Ok(new { token });

    return Results.Unauthorized();
});

app.MapPost("/validate", async (HttpRequest req) =>
{
    using var reader = new StreamReader(req.Body);
    var body = await reader.ReadToEndAsync();

    if (body.Contains(token))
        return Results.Ok();

    return Results.Unauthorized();
});

app.Run();