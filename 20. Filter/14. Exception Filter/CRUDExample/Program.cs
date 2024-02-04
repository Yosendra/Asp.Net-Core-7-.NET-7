using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Exception Filter (IExceptionFilter, IAsyncExceptionFilter)
* 
* When it runs?                 Runs when an exception is raised during the filter pipeline
* 
* OnException method            • Handles unhandled exceptions that occur in controller creation, model binding, action filters or action methods
*                               • Doesn't handle the unhandled exceptions that occur in authorization filters, resource filter, result filter or IActionResult execution
*                               • Recommended to be used only when you want a different error handling and generates different result for specific controllers;
*                                 otherwise; ErrorHandlingMiddleware is recommended over Exception Filters
* 
* ====================================================================                              
* Synchronous Exception Filter
* 
* public class FilterClassName : IExceptionFilter 
* {
*   public void OnException(ExceptionContext context)
*   {
*     // TO DO: exception handling logic here, as follows
*     context.Result = some_action_result;
*     [or]
*     context.ExceptionHandled = true;
*   }
* }
* ====================================================================
* Asynchronous Exception Filter
* 
* public class FilterClassName : IAsyncExceptionFilter 
* {
*   public async Task OnExceptionAsync(ExceptionFilterContext context)
*   {
*     // TO DO: exception handling logic here, as follows
*     context.Result = some_action_result;
*     [or]
*     context.ExceptionHandled = true;
*     
*     return Task.CompletedTask;
*   }
* }
* ====================================================================
* 
* case: we would like to handle exception that occur in PersonController method and its filter
* 
* Create HandleExceptionFilter filter, implements IExceptionFilter
* Define the logic for handling the exception in OnException
* Apply the filter in PersonController
* To simulate the exception, we are going to mismatch the database's name in appsettings.json (rename the database name to PersonsDatabase1 instead of PersonsDatabase)
* 
* Suggest: the best practice is to use the error handling middleware instead of error handling filter
* 
* Look at: HandleExceptionFilter.cs, PersonController.cs, appsettings.json
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