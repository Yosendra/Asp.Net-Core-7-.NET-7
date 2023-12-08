
// '.GetEndPoint()' returns an instance of Endpoint type, which represents an endpoint.
// That instance contains two important properties. 'DisplayName', 'RequestDelegate'

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Use(async (context, next) =>
{
    var endpoint = context.GetEndpoint();   // this will become null
    if (endpoint is not null)
    {
        await context.Response.WriteAsync($"\nEndpoint: {endpoint.DisplayName}");
    }
    await next(context);
});

app.UseRouting();

app.Use(async (context, next) =>
{
    var endpoint = context.GetEndpoint();   // this will be endpoint object, if the path is match with the routing
    if (endpoint is not null)
    {
        await context.Response.WriteAsync($"\nEndpoint: {endpoint.DisplayName}");
    }
    await next(context);
});

app.UseEndpoints(endpoints =>
{
    endpoints.MapGet("/map1", async (context) =>
    {
        await context.Response.WriteAsync("\nIn Map GET");
    });

    endpoints.MapPost("/map2", async (context) =>
    {
        await context.Response.WriteAsync("\nIn Map POST");
    });
});

app.Run(async context =>
{
    await context.Response.WriteAsync($"\nRequest received at {context.Request.Path}");
});

app.Run();
