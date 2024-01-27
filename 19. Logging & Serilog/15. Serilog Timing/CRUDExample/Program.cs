using CRUDExample.Bootstrap;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Serilog Timing
    "SerilogTimings" package records timing of a piece of your source code, indicating how much time taken for executing it.

    SerilogTimings -> Start
                       |
                       | Time taken : 00:01 second
                       |
    SerilogTimings -> End

    Install package "SerilogTimings" in Service project

    In PersonService, in GetFilteredPersons(), we want to record the time execution.

    We fix PersonServiceTest for lacking of ILogger and IDiagnosticContext injection in the constructor

Look at: PersonServiceTest.cs, PersonService.cs
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

app.UseSerilogRequestLogging();

if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
app.UseHttpLogging();
if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");
app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program {}