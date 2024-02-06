using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* UseExceptionHandler
* 
* The built-in UseExceptionHandler() middleware redirects to the specified route path
* when an unhandled exception occurs during the application execution.
* Can be used as an alternative to custom exception handling middleware.
* 
* Request -> UseExceptionHandler() (built-in) -> Custom Exception Handle Middleware -> Routing Middleware -> Endpoint Middleware
* • Inside UseExceptionHandler()
* Catches & Logs unhandled exceptions.
* Re-execute the request in an alternatives pipeline using the specified route path.
* 
* Difference between custom exception middleware and built-in exception middleware
* in custom exception middleware we can write our own code such as logging the error into serilog, can write the status code, response message, etc
* in built-in exception middleware is mainly for redirecting to a specific route when an exception occurs
* It is recommended to put the built-in exception handler at the most first middleware sequence
* 
* To simulate the error write the wrong database's name at appsettings.json
* Apply the UseExceptionHandler in Program.cs, remember put it in most top
* 
* We add HomeController, adding Error() action method to put the Error page
* Add the Error.cshtml
* 
* In ExceptionHandlingMiddleware, in catch block, throw again the exception to be catched by built-in ErrorHandler middleware
* so it can be redirected to our Error page we have created
* 
* Look at: appsettings.json, Program.cs, HomeController.cs, ExceptionHandlingMiddleware.cs
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
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");              // notice this, all type of exception will redirected to this path
    app.UseExceptionHandlingMiddleware();
}
app.UseSerilogRequestLogging();
app.UseHttpLogging();
if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

public partial class Program {}