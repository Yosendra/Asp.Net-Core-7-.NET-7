// We are using Postman here to make a custom request.
// In Postman we can create our own request header key which is not possible if we are using browser.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    context.Response.Headers["Content-type"] = "text/html";

    string ourCustomKey = "Our-Custom-Key"; // here is our custom request header key
    if (context.Request.Headers.ContainsKey(ourCustomKey))
    {
        string ourCustomKeyValue = context.Request.Headers[ourCustomKey];
        await context.Response.WriteAsync($"<p>{ourCustomKeyValue}</p>");
    }
});

app.Run();
