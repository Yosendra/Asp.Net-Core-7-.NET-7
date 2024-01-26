using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Logging Providers (Console, Debug, EventLog)

    Log automatically displayed in Console (Kestrel Console) and Debug (In Visual Studio -> Output -> Debug)

    We try to change log level for EventLog, we customize it in appsettings.Development.json -> EventLog:LogLevel:Default

    We customize our log provider in Program.cs

Look at: appsettings.Development.json, Program.cs
*/

var builder = WebApplication.CreateBuilder(args);

// Logging
builder.Host.ConfigureLogging(loggingProvider =>
{
    loggingProvider.ClearProviders();   // Remove all Logging provider (Debug, Console, EventLog)
    loggingProvider.AddConsole();       // Add Console as our Logging provider, now Console is the only one place we can see the log
});

builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddRepositories()
    .AddServices()
    .AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

// All these messages can be seen in Kestrel console when running the web application
app.Logger.LogDebug("debug-message");   // except debug and trace because the default log level of asp.net core application is set to information
app.Logger.LogInformation("information-message");
app.Logger.LogWarning("warning-message");
app.Logger.LogError("error-message");
app.Logger.LogCritical("critical-message");

if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");

app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program {}