using CRUDExample.Bootstrap;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Filters
  are the code block that execute before or after specific stages in "Filter Pipeline"
  Filters perform specific task such as authorization, caching, exception handling, etc.

  Action Filter
  We add Filter folder in UI project, this is where we place our filter class.
  We add ActionFilters sub-folder to place specific to ActionFilter classes.
  
  Create PersonListActionFilter class in that sub-folder.
  To make PersonListActionFilter class to be ActionFilter class, this class has to inherit from IActionFilter.
  After that implement the interface.
  Inject some services you needed in the constructor.

  We need to add this ActionFilter class to the particular action method in order to use it.
  In this case we want to place it in Index() in PersonController by adding [TypeFilter] attribute
  You may test this by putting breakpoint in both "OnActionExecuted" and "OnActionExecuting" methods and "PersonController.Index"

  When it runs?
    Runs immediately before and after an action method executes.
  OnActionExecuting method
    • It can access the action method parameters, read them & do necessary manipulation on them
    • It can validate action method parameters
    • It can short-circuit the action (prevent action method from executing) and return a different IActionResult
  OnActionExecuted method
    • It can manipulate the ViewData
    • It can change the result returned from the action method
    • It can throw exception to either return the exception to the exception filter (if exist) or return the error response to the browser

Look at: PersonListActionFilter.cs, PersonController.cs
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