
// We can use reverse proxy server called IIS Express
// Look at Properties -> launchSettings.json
//
// Profile : a collection of settings which enables a particular server to run our application when we start project.
// "applicationUrl": "http://localhost:5023".       The port number range 1024 to 65536
// "environmentVariables": {                        Contains the global values that are available accross entire application
//      "ASPNETCORE_ENVIRONMENT": "Development"
//  }

var builder = WebApplication.CreateBuilder(args);
var app = builder.Build();

app.MapGet("/", () => "Hello World!");

app.Run();
