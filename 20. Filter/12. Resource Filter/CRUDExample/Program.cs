using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Resource Filter 
* When it runs?                 Runs immediately after Authorize Filter and after Result Filter executes
* 
* OnResourcexecuting method     It can do some work before model binding. Eg: Adding metrics to an action method
*                               It can change the way how model binding works (invoking a custom model binder explicitly)
*                               It can short-circuit the action (prevent IActionResult from execution) and return a different IActionResult. Eg: Short-Circuit if unsupported content type is requested
* 
* OnResourceExecuted method     It can read the response body and store it in cache
*                               
* Case we want resource filter that temporarily disables an action method of Create (POST)                             
* Create custom filter FeatureDisableResourceFilter
* Use the FeatureDisableResourceFilter at Create (POST) of PersonController
* 
* When the user try to make request to Create (POST), user will see error message in Create view instead
* Comment out the javascript tag in Create Person view
* Test it by making request to create person with empty fields
* 
* Look at: FeatureDisableResourceFilter.cs, PersonController.cs, 
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