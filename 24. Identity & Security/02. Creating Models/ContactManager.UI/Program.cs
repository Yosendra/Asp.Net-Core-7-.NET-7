using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Identity Models
* 
* IdentityUser<T>
*   Act as a base class for ApplicationUser class that      |  Built-in Properties:
*   acts as model class to store user details.              |  Id               Email
*                                                           |  Username         PhoneNumber
*   You can add additional properties to the                |  PasswordHash
*   ApplicationUser class.                                  |
*   
* 
* IdenityRole<T>
*   Acts as a base class for ApplicationRole class that     |  Built-in Properties:
*   acts as model class to store role details. Eg: "admin"  |  Id
*                                                           |  Name
*   You can additional properties to the                    |  
*   ApplicationRole class                                   |
*   
*   
* Add folder IdentityEntities at Core project inside Domain folder
* Add ApplicationUser class inherit to IdentityUser<T>
* Add ApplicationRole class inherit to IdentityRole<T>
* Install package Microsoft.AspNetCore.Identity in Core project
* Install package Microsoft.AspNetCore.Identity.EntityFrameworkCore in Core project
* Install package Microsoft.AspNetCore.Identity.EntityFrameworkCore in UI project
* 
* At ApplicationDbContext, change where it inherit from "DbContext" to "IdentityDbContext<TUser, TRole, TKey>"
* Install package Microsoft.AspNetCore.Identity.EntityFrameworkCore in Infrastrutcture project
* 
* Look at: ApplicationUser.cs, ApplicationRole.cs, ApplicationDbContext.cs
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