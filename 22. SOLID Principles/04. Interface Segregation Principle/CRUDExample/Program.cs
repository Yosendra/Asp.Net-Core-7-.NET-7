using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Interface Segregation Principle
* Instead of writing one large interface with so many methods, we group them into multiple interfaces with fewer methods
* 
* public interface Interface1
* {
*   void Method1(); // perform one task
*   void Method2(); // perform a different task
* }
* 
* -should be re-written as-
* public interface Interface1
* {
*   void Method1();
* }
* public interface Interface2
* {
*   void Method2();
* }
* 
* No client class should be forced to depend on methods it doesn't use.
* We should prefer to make many smaller interface rather than one single big interface.
* 
* • The client class may choose one or more interfaces to implement
* • Benefit: makes it easy to crete alternative implementation for a specific functionality, rather than recreating entire class
* 
* Here we break the IPersonService into small interface
* Notice we eventhough we are breaking the interface, we are not going to use it
* It just for the sake of learning
* 
* Look at: IPersonGetterService.cs, 
*          IPersonAdderService.cs, 
*          IPersonUpdaterService.cs, 
*          IPersonDeleterService.cs, 
*          IPersonSorterService.cs
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