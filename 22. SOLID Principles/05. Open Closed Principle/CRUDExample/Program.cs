using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Open Closed Principle
* 
* A class is closed for modification, but open for extension
* You should treat each class as readonly for development means; unless for bug-fixing
* 
* • if you want to extend / modify the functionality of an existing class; you need to recreate it as a seperate & alternative
*   implementation; rather than modifying existing code of the class
* • Benefit: not modifying existing code of a class doesn't introduce new bugs and keeps the existing unit test stay
*   relevant and needs no changes.
* 
* Suppose there is some changes at GetPersonExcel() in PersonService
* Instead of modifying it, we are going to extend it by creating a new class implementing the changes we want in GetPersonExcel()
* Create PersonGetterServiceWithFewExcelFields class in Service project implementing IPersonGetterService interface
* Create field for existing PersonGetterService class at PersonGetterServiceWithFewExcelFields and inject it through constructor 
*   (look at the constructor, we are using class here, not interface)
* Define our new logic for GetPersonExcel(), for other new PersonService methods we are calling from existing PersonService methods
* Register the new PersonService to IoC container (We are not doing it, it just for learning sake)
* 
* Look at: IPersonGetterService.cs, 
*          PersonService.cs
*          PersonGetterServiceWithFewExcelFields.cs
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
    app.UseExceptionHandler("/Error");
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