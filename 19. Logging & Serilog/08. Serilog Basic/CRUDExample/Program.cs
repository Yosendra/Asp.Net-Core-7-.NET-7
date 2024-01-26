using CRUDExample.Bootstrap;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Serilog
    Serilog is a structured logging library for Asp.Net Core
    Supports variety of logging destinations, referred as "Sinks" - starts with Console, Azure, DataDog, ElasticSearch, Amazon CloudWatch, Email, and Seq

    .Net Core App ---> Serilog (Logging Framework) ---> Logs (Console, Debug, EventLog, File, Database, Seq, Azure)

    Why do we have third-party library for logging meanwhile we already have asp.net core built-in logging framework?
    Because there are some drawbacks in asp.net core built-in logging framework. It doesn't support you to log to other external log destination.
    Such as file and database.
    
    We need to install "Serilog" and "Serilog.AspNetCore" package in our UI project (CRUDExample)

    Replace our previous built-in logging framework with Serilog by using "builder.Host.UseSerilog()" in Program.cs

    We add "Serilog" configuration key in appsettings.json for used by Serilog

    Serilog - Configuration
    {
        "Serilog" : {
            "Using": [
                "Serilog.Sinks.YourSinkHere"
            ],
            "MinimumLevel": "Debug | Information | Warning | Error | Critical",
            "WriteTo": [
                {
                    "Name": "YourSinkHere",
                    "Args": "YourArguments",
                }
            ]
        }
    }

    You may run the web app and see now the log handled by Serilog.
    You may change the log level for Serilog to "Information"

Look at: Program.cs, appsettings.json
*/

var builder = WebApplication.CreateBuilder(args);
//builder.Host.ConfigureLogging(loggingProvider =>
//{
//    loggingProvider
//        .ClearProviders()
//        .AddConsole()
//        .AddDebug()
//        .AddEventLog();
//});
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)   // read configuration from appsettings.json
        .ReadFrom.Services(service);                     // make our services available to Serilog
});
builder.Services
    .AddHttpLogging(options => 
    {
        options.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
    })
    .AddDbContext<ApplicationDbContext>(options => 
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    })
    .AddRepositories()
    .AddServices()
    .AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
app.UseHttpLogging();
if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");
app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program {}