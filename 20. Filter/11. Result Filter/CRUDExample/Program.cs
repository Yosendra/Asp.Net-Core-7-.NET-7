using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Result Filter
* When it runs?                 Runs immediately before and after an IActionResult executes
* 
* OnResultExecuting method      It can access the IActionResult returned by the action method
*                               It can continue executing the IActionResult normally by not assigning "Result" property of the context
*                               It can short-circuit the action (prevent IActionResult from execution) and return a different IActionResult
* 
* OnResultExecuted method       It can manipulate the last-moment changes in the response, such as adding necessary response header
*                               It should not throw exception because exceptions raised in result filter would not be caught by the exception filter
*                               
* Case : In PersonController.Index(), we want to add reponse header after the view executes
* We create custom filter "PersonListResultFilter"
* Add our logic of adding custom Response Header inside "OnResultExecutionAsync" in "PersonListResultFilter" class
* Apply this filter in PersonController.Index
* Put breakpoint, in BEFORE and AFTER logic in the filter
* 
* Look at: PersonListResultFilter.cs, PersonController.Index()
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
    .AddControllersWithViews(options =>
    {
        var logger = builder.Services
            .BuildServiceProvider()
            .GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

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