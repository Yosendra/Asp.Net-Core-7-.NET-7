using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Exception Handling Middleware
* 
* Handles all errors occured in filter pipeline (including model binding, controllers, and filters)
* Should be added to the application pipeline before Routing Middleware
* 
* Exception Handling Middleware -> Other Middleware -> Routing Middleware (action selection) -> Endpoint Middleware (action execution)
* 
* Inside Endpoint Middleware
* Filter Pipeline : Filter (before) -> Action method -> Fitler (after)
* 
* 
* Test : simulate exception by giving wrong database name in appsettings.json
* Comment out HandleExceptionFilter filter at PersonController
* Change the environment ASPNETCORE_ENVIRONMENT value to "Production"
* The error page will be 500 internal server error, empty page without message to trace, not developer friendly
* 
* Error Handling Filter is limited to only specific request 
* We want to handle this error in application-level scope (all request) by using Error Handling Middleware
* 
* Create our error handling middleware, ExceptionHandlingMiddleware class
* Use this middleware in Program.cs
* Apply the middleware at most top middleware, this is a must, to make all exception can be catched by the middleware
* Put the breakpoint at the error handling middleware, then run the application
* 
* Look at: appsettings.json, ExceptionHandlingMiddleware.cs, Program.cs
*/

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(service);
});
builder.Services.ConfigureServices(builder.Configuration);


var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
else
    app.UseExceptionHandlingMiddleware();       // Apply our ErrorHandlingMiddleware beside Development environment
app.UseSerilogRequestLogging();
app.UseHttpLogging();
if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

public partial class Program {}