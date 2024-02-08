using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* UI
* 
* Add project reference to Core project
* Note: we add project reference to Infrastructure only because we need ApplicationDbContext class in dependency injection
*       since dependency injection happen in UI project more specificly in ConfigureServiceExtension class
* 
* Install all the package needed in UI Project
* Change database name to "ContactDatabase" in connection string in appsettings.json
* Migrate database. Go to Pacakage Manager Console, change the default project to "ContactManager.Infrastructure", then type "Add-Migration Initial"
* Next type "Update-Database"
* Run the application
* 
* Look at: appsettings.json
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