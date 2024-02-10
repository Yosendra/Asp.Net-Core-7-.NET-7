using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Authorization Policy
* 
* Requirement: All pages include person page, upload country, create, edit, delete person page only accessible after
*              the user has login, if the user has not login, the remaining page is unaccessible and redirected
*              to login page.
* 
* To do so, we need authorization policy to the service container to enforce the requests to have the Identity cookie
* If the browser not submitter the Identity cookie, it is assumed as the user not logged in, then automatically
* be redirected to login url
* 
* note: there is correction in the sequence of Authentication middleware
* 
* Add Authorization middleware to request pipeline
* Add Authorization service in service container
* Add Login path also at ConfigureServicesExtension
* 
* Also we want Account and Home controllers can be accessed without login
* On AccountController class apply attribute [AllowAnonymous] 
* 
* Look at: Program.cs, ConfigureServicesExtension.cs, AccountController.cs, HomeController.cs
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
app.UseAuthentication();        // moved to after UseRouting()
app.UseAuthorization();         // add authorization middleware, to evaluate the access permission whether the user has access or not
app.MapControllers();
app.Run();

public partial class Program {}