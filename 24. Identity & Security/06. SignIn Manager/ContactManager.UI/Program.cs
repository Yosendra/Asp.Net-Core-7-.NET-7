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
* Inject SignInManager class in AccountController
* In Register() action method, defined our code for Sign-In process after success registering user
* 
* In _Layout.cshtml notice we can access User property in the view, it represents the logged-in user
* Add Authentication middleware in request pipeline for reading Authentication cookie
* In RegisterDto we add annotation [Compare] for ConfirmPassword property
* 
* Run the applicatiion, then try to register through registration page, take a look at the UserName displayed at right corner page
* 
* Look at: AccountController.cs, _Layout.cshtml, Program.cs, RegisterDto.cs
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
app.UseAuthentication();                                    // notice this. Reading Identity cookie
app.UseRouting();                                           // identifying action method based on route
app.MapControllers();                                       // executing the filter pipeline (action + filter)
app.Run();

public partial class Program {}