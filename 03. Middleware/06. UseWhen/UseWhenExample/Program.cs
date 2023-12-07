var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// If the first parameter return 'true', then run the middleware we define in the second parameter
app.UseWhen(context => context.Request.Query.ContainsKey("username"), // if there is 'username' in query string, return true
    app => 
    {
        app.Use(async (context, next) =>    // execute this middleware if there is 'username' in query string 
        {
            await context.Response.WriteAsync("Hello from middleware branch");
            await next(context);    // go to next middleware
        }); 
    }
);

app.Run(async context =>
{
    await context.Response.WriteAsync("\nHello from middleware at main chain");
});

app.Run();
