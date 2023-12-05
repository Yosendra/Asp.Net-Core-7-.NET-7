// Query String -> a syntx where you can send the parameter values from the request to the server.
//  i.e. /dashboard?id=1, id=1 is the query string.
//  The path (dashboard) and query string (id=1) is seperated by question mark symbol (?)

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    context.Response.Headers["Content-type"] = "text/html";
    string userAgentKey = "User-Agent";
    // Don't worry, response and request header key is case insensitive
    if (context.Request.Headers.ContainsKey(userAgentKey))
    {
        string userAgent = context.Request.Headers[userAgentKey];
        await context.Response.WriteAsync($"<p>{userAgent}</p>");
    }
});

app.Run();
