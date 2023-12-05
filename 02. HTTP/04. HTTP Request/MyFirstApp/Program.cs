// HTTP Request -> is a message that is sent from browser to server
// 2 type of request method :
//  POST -> Would contain request body.
//  GET  -> Request body empty.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{
    string path = context.Request.Path;                 // You can see this in starting line of Request Header in dev tools' browser    i.e.    /, /course, /user
    string requestMethod = context.Request.Method;      // You can see this in starting line of Request Header in dev tools' browser    i.e.    GET, POST

    context.Response.Headers["Content-type"] = "text/html";

    await context.Response.WriteAsync($"<h1>{path}</h1>");
    await context.Response.WriteAsync($"<h1>{requestMethod}</h1>");
});

app.Run();
