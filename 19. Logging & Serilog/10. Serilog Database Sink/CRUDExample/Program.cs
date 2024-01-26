using CRUDExample.Bootstrap;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Serilog - MSSqlServer Sink
    
    The "Serilog.Sinks.MSSqlServer" logs into a specified SQL Server database.
    You can configure the connection string using configuration settings.

    .Net Core App -> Serilog -> SQL Server Database Logs

    Install package "Serilog.Sinks.MSSqlServer"

    Add Serilog configuration for using MSSqlServer as sink database. (take a look at the "Using" and "WriteTo" key)

    We define the connectionString for our sink. It is same instance as our connectionString but we choose different database specific for storing the logs.
    Create the database used for sink. In this case we name it "CRUDLogs"
    
    You may run the application, then you can see the log written CRUDLogs database

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