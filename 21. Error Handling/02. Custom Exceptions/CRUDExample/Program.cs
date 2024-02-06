using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Custom Exception class
* 
* A custom exception class is an exception that inherits from System.Exception class & represents a domain-specific exception
* Used to represent the domain-specific error stand-out of system-related (.NET) related exception
* 
* System.Exception class
*   |
*   | Inherits
*   |
* Custom Exception class
* 
* For example, in PersonService.UpdatePerson(), if the argument supplied is exist or not valid, we don't want to throw ArgumentException
* instead we want to throw custom exception because ArgumentException is not specific to the domain, it is difficult to differentiate it 
* with the others method which also throw ArgumentException
* 
* It is recommended by Microsoft to define the custom exception with 3 constructor overloading like this
* 
* public class CustomException : Exception
* {
*     public CustomException() : base()
*     {
*     }
*     
*     public CustomException(string? message) : base(message)
*     {
*     }
*     
*     public CustomException(string? message, Exception? innerException) : base(message, innerException)
*     {
*     }
* }
* 
* Create a project named "Exception" for containing our custom exception
* Add our custom exception class to that project named "InvalidPersonIdException" inherit from "ArgumentException"
* Define the constructor like above as the recomended by Microsoft
* Add this project reference to the service project and main project
* 
* In PersonService we apply the custom exception we have created in PersonUpdate method
* In PersonController at Edit() action method we add a statement to change the existing id in requestModel object
* This will simulate the InvalidPersonIdException
* Make sure the database name is correct in appsettings.json
* Put the breakpoint at ExceptionErrorHandlingMiddleware at the catch block
* Go to Edit person page, then submit in that Edit person form
* 
* Wherever you feel there is a significant need of representing a specific or situational error
* then that is the motivation to create your own custom exception class
* 
* Look at: InvalidPersonIdException.cs, PersonService.cs, PersonController.cs, appsettings.json
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
    app.UseExceptionHandlingMiddleware();
app.UseSerilogRequestLogging();
app.UseHttpLogging();
if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

public partial class Program {}