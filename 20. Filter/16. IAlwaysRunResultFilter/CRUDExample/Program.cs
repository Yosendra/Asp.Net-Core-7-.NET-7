using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* IAlwaysRunResultFilter
* 
* case: We want to test, is AlwaysRunResultFilter always be executed
* 
* Create PersonAlwaysRunResultFilter, implements IAlwaysRunResultFilter
* Apply PersonAlwaysRunResultFilter at Edit (POST) in PersonController
* Comment out the TokenResultFilter at Edit (GET) in PersonController
* Put breakpoint at PersonAlwaysRunResultFilter on both methods
* Run the application
* Go to Edit person view, then submit the form
* 
* Look at: PersonAlwaysRunResultFilter.cs
*/

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(service);
});
builder.Services.AddHttpLogging(configureOptions => 
{
    configureOptions.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
})
.AddDbContext<ApplicationDbContext>(options => 
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
})
.AddRepositories()
.AddServices()
.AddControllersWithViews(options => 
{
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
    options.Filters.Add(new ResponseHeaderActionFilter(logger, "Key-From-Global", "Value-From-Global", 2));
});

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