using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Areas
* 
* Area is a group of related controllers, views, and models that are related to specific module or specific user
* 
*   ASP.NET Core Application
*         Area1            Area2             Area3
*       • Controllers     • Controllers     • Controllers
*       • Views           • Views           • Views
*       • Models          • Models          • Models
*   
* We are going create area in UI project.
* Right click at the project, add new scaffold item, choose MVC area
* Before that, Install manually Install package "Microsoft.VisualStudio.Web.CodeGeneration.Design" the same version as follows:
*   • Microsoft.EntityFrameworkCore
*   • Microsoft.EntityFrameworkCore.SqlServer
*   • Microsoft.EntityFrameworkCore.Tools
*   • Microsoft.AspNetCore.Identity.EntityFrameworkCore
* In this case we are using version 6.0.9
* 
* In Area -> Admin -> Controllers, right click on Controller folder, add controller "HomeController"
* At the HomeController class give attribute [Area("Admin")] to mark it as the Area for "Admin"
* 
* We can use conventional routing for Area.
* Add middleware UseEndpoint(), we can see below
* 
* Add view of Index action method of HomeController (Admin)
* In _Layout.cshtml, add one more list for admin, give condition it appears only when the User is an admin
* In Admin area view, all the layout, _ViewImports.cshtml, _ViewStart.cshtml is seperated from the regular one, so we need to create it again for this area
* In _ViewImports.cshtml (Admin) import the tag helper in order to be able to use tag helper in Index.cshtml (Admin)
* 
* Test -> Login as Admin, the menu to Admin Home appears
*         Login as User, the Admin Home menu doesn't appear
* 
* Look at: Program.cs, HomeController.cs (Admin), Index.cshtml (Admin), _Layout.cshtml, 
*          _ViewImports.cshtml (Admin)
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
        pattern: "{area:exists}/{controller=Home}/{action=Index}");    // ex: Admin/Home/Index, give constaint "exists" for area
                                                                       // give default value so when access /Admin it equals to "/Admin/Home/Index"
});

app.Run();

public partial class Program {}