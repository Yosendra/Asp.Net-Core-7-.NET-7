using CRUDExample.Bootstrap;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Serilog - IDiagnosticContext
    Diagnostic context allows you to add additional enrichment properties to the context 
    and all those properties are logged at once in the final "log completion event" of the request

                                  Log (normal)                  
                                  --------------------->          
             Request
    Browser -------> Asp.Net Core                            Serilog  --------------------->  Serilog Sink

                                  Log Completion
                                  [diagnostic context props]
                                  --------------------->

    Install package "Serilog.Extensions.Hosting" at Service project    

    Inject "IDiagnosticContext" at PersonService

    Invoke "IDiagnosticContext" method in GetFilteredPersons()

    Use middleware ".UseSerilogRequestLogging()" to enable the endpoint completion log.
    It adds an extra log message as soon as the request-response completed.

    We override toString() method in Person entity class for logging purpose

Look at: Program.cs, appsettings.json, PersonService.cs, Person.cs
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

app.UseSerilogRequestLogging(); // Add this middleware for IDiagnosticContext

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
app.UseHttpLogging();
if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");
app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program {}