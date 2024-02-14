using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* XSRF - Part 1
* 
* XSRF (Cross Site Request Forgery - CSRF) is a process of making a request to a web server from another domain
* using an existing authentication of the same web server.
* Eg: attacker.com creates a form that sends malicious request to original.com
* 
* We apply [ValidateAntiForgeryToken] attribute at Register action method at AccountController
* In Register.cshtml, at the register form, we have to use form tag helper
* 
* Apply [AllowAnonymous] attribute at IsEmailAlreadyRegistered() endpoint
* 
* Insight - don't forget to use [ValidateAntiForgeryToken] in POST action method. It also only works for POST
* 
* We can use validate anti forgery token globally through filter
* Look at ConfigureServicesExtension, at AddControllersWithViews()
* 
* Look at: AccountController.cs, Register.cshtml, ConfigureServicesExtension.cs
*/

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((context, service, loggerConfiguration) => 
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(service);
});


// Add services to the container.
builder.Services.ConfigureServices(builder.Configuration);
var app = builder.Build();


// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}
app.UseHsts();
app.UseHttpsRedirection();              
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