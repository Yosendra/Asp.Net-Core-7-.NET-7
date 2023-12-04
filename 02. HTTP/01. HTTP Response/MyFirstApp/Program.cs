
var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

// "Hello World!" -> is the response body
app.MapGet("/", () => "Hello World!");

app.Run();
