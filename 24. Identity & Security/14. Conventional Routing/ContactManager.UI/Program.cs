using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Conventional Routing
* 
* Conventional routing is a type of routing system in ASP.NET Core that defines route templates
* applied on all controllers in the entire application.
* You can override this using attribute routing on a specific action method.
* 
* endpoints.MapControllerRoute(
*   name: "default",
*   pattern: "{controller=Person}/{action=Index}/{id?}"
* );
* 
* Use Conventional Routing when all endpoint url are same.
* Routing attribute give more flexibiliy rather than conventional routing.
* Industry is starting move to Route routing. So Routing attribute is more prefered to be used.
* We are only use this only this once
* 
* In Program.cs, we apply UseEndpoints() middleware to define conventional routing
* We comment out routing attribute in AccountController
* 
* Test by accessing Login or Register page, and it still accessible due to Conventional Routing
* 
* Look at: Program.cs, AccountController.cs
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
        name: "default",
        pattern: "{controller}/{action}/{id?}"
    );
});

app.Run();

public partial class Program {}