using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* User Roles
* 
* Create "UserTypeOptions" enum for representing User Role we want to use in Authorization
* In RegisterDto, add new property "UserType"
* In Register.cshtml we add radio button at the Register form
* In AccountController we inject RoleManager<ApplicationRole>
* In Register (POST) action method of AccountController, we add new logic for adding role and assign role when register success
* 
* Test: Register new user with valid selected Role, then check the table of AspNetUsers, AspNetRoles, AspNetUserRoles
* 
* Look at: UserTypeOptions.cs, RegisterDto.cs, Register.cshtml, AccountController.cs
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
app.Run();

public partial class Program {}