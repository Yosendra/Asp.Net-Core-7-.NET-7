// Query String -> a syntx where you can send the parameter values from the request to the server.
//  i.e. /dashboard?id=1, id=1 is the query string.
//  The path (dashboard) and query string (id=1) is seperated by question mark symbol (?)
//  If you want to attach more than 1 value, you have to attach '&' symbol after each variabel query string. i.e. /dashboard?id=1&name=scott

// In GET request, the data we sent will be attached to this query string.
// whereas in POST request, the data we sent will be in request body. There is no query string.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    // Make sure we are calling .WriteAsync() after we define the header response's content-type
    context.Response.Headers["Content-type"] = "text/html";
    if (context.Request.Method == "GET")
    {
        // Query string data. Collection of Key-Value Pair
        var collection = context.Request.Query;
        if (collection.ContainsKey("id"))
        {
            string id = context.Request.Query["id"];
            await context.Response.WriteAsync($"<h1>{id}</h1>");
        }
    }
});

app.Run();
