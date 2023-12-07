
// The extension method called '.Use()' is used to execute a non-terminating / shor-circuiting middleware
// that may /may not forward the request to the next middleware.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// middleware 1
app.Use(async (HttpContext context, RequestDelegate next) =>
{   
    await context.Response.WriteAsync("Hello");
    await next(context);
});

// middleware 2
app.Use(async (context, next) => 
{
    await context.Response.WriteAsync("Hello again!");
    await next(context);    // If we decide this middleware become the end, just don't call this '.next(context)' statement
});

// middleware 3 (terminating middleware)
app.Run(async (context) =>
{
    await context.Response.WriteAsync("Hello again and again!");
});

app.Run();
