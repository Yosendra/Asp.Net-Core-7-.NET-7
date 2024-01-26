using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* ILogger in Controller

    We try to inject logger in PersonController
    We try to inject logger in PersonService
    We try to inject logger in PersonRepository

    Once we run the web app, we could see the logs in Kestrel Console

Look at: PersonController.cs, PersonService.cs, PersonRepository.cs
*/

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(loggingProvider =>
{
    loggingProvider
        .ClearProviders()
        .AddConsole();
});

builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddRepositories()
    .AddServices()
    .AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

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