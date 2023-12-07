
// Middleware class is used to seperate the middleware logic from a lambda expression to a seperate / reusable class.

using MiddlewareExample.CustomeMiddleware;

var builder = WebApplication.CreateBuilder(args);

// in this builder object we have 'Services' property. Its type is IServicesCollection 
// which can hold the list of services that can participate in the dependency injection
builder.Services.AddTransient<MyCustomMiddleware>();

var app = builder.Build();

// middleware 1
app.Use(async (HttpContext context, RequestDelegate next) =>
{   
    await context.Response.WriteAsync("From middleware 1");
    await next(context);
});

// middleware 2, Here we decide to use our custom middleware by using '.UseMiddleware<T>()'
app.UseMiddleware<MyCustomMiddleware>();

// middleware 3 (terminating middleware)
app.Run(async (context) =>
{
    await context.Response.WriteAsync("\nFrom middleware 3");
});

app.Run();
