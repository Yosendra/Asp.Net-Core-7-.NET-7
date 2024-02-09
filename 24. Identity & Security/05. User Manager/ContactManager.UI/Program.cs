using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Manager
* 
* • UserManager                                             |   Methods:
* Provides business logic methods for managing users.       |   CreateAsync()       FindByEmailAsync()
* It provides methods for creating, searching, updating,    |   DeleteAsync()       FindByIdAsync()
* and deleting users                                        |   UpdateAsync()       FindByNameAsync()
*                                                           |   InInRoleAsync()
*                                                           
* • SignInManager
* Provides business logic methods for sign-in and sign-in   |   Methods:
* functionality of the users. It provides methods for       |   SignInAsync()
* creating, searching, updating, and deleting users.        |   PasswordSignInAsync()
*                                                           |   SignOutAsync()
*                                                           |   IsSignIn()
*                                                           
* Inject UserManager class in AccountController
* In Register() action method, defined our code for registering User
* 
* Migrate the database to provide table for Identity to store the user data
* Go to Package Manager Console, default project to Infrastructure, run these commands:  
*   "Add-Migration IdentityTables"
*   "Update-Database"
* Now the additional tables for Identity to be used are generated
* 
* Run the applicatiion, then try to register through registration page
* 
* Look at: AccountController.cs
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