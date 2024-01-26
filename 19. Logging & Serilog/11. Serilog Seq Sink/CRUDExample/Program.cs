using CRUDExample.Bootstrap;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Serilog - Seq Sink
    The "Serilog.Sinks.Seq" is a real-time search and analysis server for structured application log data.
    It can run on Windows, Linux, or Docker

    .Net Core App -> Serilog -> Seq server

    The main reason to use Seq is that it offers the monitoring tools to monitor log of the application in real time.
    We need to install Seq software since it is third-party tool
    After installing the software and add configuration, install "Serilog.Sinks.Seq" package
    Now add configuration for Seq in appsettings.json in Serilog group

    Serilog makes HTTP request to write log in particular Seq server,
    the Seq server receives the log from the Serilog and makes it available in the monitoring tool.

    You may run the web application, then see the log in Seq monitoring server

Look at: appsettings.json
*/

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(service);
});
builder.Services
    .AddHttpLogging(configureOptions => 
    {
        configureOptions.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
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