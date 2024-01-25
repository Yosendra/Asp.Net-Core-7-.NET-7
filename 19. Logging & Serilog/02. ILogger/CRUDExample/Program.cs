using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* ILogger
  
Debug
  ILogger.LogDebug("Log Message")           Logs that provide details & values of variables for debugging purpose

Information
  ILogger.LogInformation("Log Message")		Logs that track the general flow of the application execution

Warning
  ILogger.LogWarning("Log Message")		    Logs that that highlight an abnormal or unexpected event

Error
  ILogger.LogError("Log Message")		    Logs to indicate that flow of execution is stopped due to a failure

Critical
  ILogger.LogCritical("Log Message")		Logs to indicate an unrecoverable application crash

Look at: Program.cs, invocation of Logger.Log...

*/

var builder = WebApplication.CreateBuilder(args);
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