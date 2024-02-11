using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Custom Authorization Policies
* 
* Case: We want AccountController action methods should be accessible only if the user not logged in
* 
* Add policy in AddAuthorization in Service Container in ConfigureServicesExtension
* We apply [Authorize] attribute with "NotAuthenticated" policy in these action methods at AccountController:
*   • Login (GET)
*   • Login (POST)
*   • Register (GET)
*   • Register (POST)
* 
* Apply [Authorzie] at Logout, it means this only accessible only for logged-in user, otherwise it isn't accessible
* Delete [AllowAnonymous] attibute at AccountController
* 
* Test: Login as User or Admin, then access "/Account/Login", access will be denied
* 
* Look at: ConfigureServicesExtension.cs, AccountController.cs
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
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();
app.UseEndpoints(endpoints =>
{
    endpoints.MapControllerRoute(
        name: "areas",
        pattern: "{area:exists}/{controller=Home}/{action=Index}");
});
app.Run();

public partial class Program {}