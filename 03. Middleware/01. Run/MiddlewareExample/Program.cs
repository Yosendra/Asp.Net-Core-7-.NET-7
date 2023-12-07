// The extension called 'Run()' is used to execute a terminating / shor-circuiting middleware
// that doesn't forward the request to the next middleware.

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.Run(async (context) =>
{   // The lambda expression we pass in Run itself is called as middleware,
    // which is called request delegate technically
    await context.Response.WriteAsync("Hello");
});

// This will not be executed because the nature of Run() doesn't forward the request to the subsequent middleware
app.Run(async (context) =>
{   // The lambda expression we pass in Run itself is called as middleware,
    // which is called request delegate technically
    await context.Response.WriteAsync("Hello Again");
});

app.Run();
