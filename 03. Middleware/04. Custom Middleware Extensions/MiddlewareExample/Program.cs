
// Middleware extension method is used to invoke the middleware with a single method call

using MiddlewareExample.CustomeMiddleware;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddTransient<MyCustomMiddleware>();
var app = builder.Build();

// middleware 1
app.Use(async (HttpContext context, RequestDelegate next) =>
{   
    await context.Response.WriteAsync("From middleware 1");
    await next(context);
});

// middleware 2, Use the extension method of our custom middleware
app.UseMyCustomMiddleware();

// middleware 3 (terminating middleware)
app.Run(async (context) =>
{
    await context.Response.WriteAsync("\nFrom middleware 3");
});

app.Run();
