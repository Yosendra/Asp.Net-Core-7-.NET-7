using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* HTTP Logging

    Logs detail all HTTP request and response.
    You need to set a value of "HttpLoggingFields" enum to set specify desired details.

             request 1                                request 1
             ---------->                              ---------->    1. request & response details
             request 2                                request 2
     Browser ---------->    HTTP Logging Framework    ---------->    2. request & response details
             request 3                                request 3
             ---------->                              ---------->    3. request & response details

    To enable HTTP logging, we can use this middleware "app.UseHttpLogging()"

    Set the minimum log level of "Microsoft.AspNetCore" appsettings.Development.json into "Information"

    Now, every request-response log can be seen in Kestrel Console when we make a request to our web application

Look at: Program.cs
*/

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(loggingProvider =>
{
    loggingProvider
        .ClearProviders()
        .AddConsole()
        .AddDebug()
        .AddEventLog();
});

builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddRepositories()
    .AddServices()
    .AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseHttpLogging();   // enable HTTP Logging

//app.Logger.LogDebug("debug-message");
//app.Logger.LogInformation("information-message");
//app.Logger.LogWarning("warning-message");
//app.Logger.LogError("error-message");
//app.Logger.LogCritical("critical-message");

if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");

app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program {}