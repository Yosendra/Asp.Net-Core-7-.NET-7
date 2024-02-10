using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Remote Validation
* 
* Problem : when registering the already existing email, we will get the the same register page with error notification that 
*           email already access. The whole same page gets reloaded in this process. This is not necessary.
* Solution : we will make asynchronous request when the email is entered in the form
* 
* We add IsEmailAlreadyRegistered action method in AccountController returning json with true or false
* We need to install package "Microsoft.AspNetCore.Mvc.ViewFeatures" at Core project to be able using [Remote] attribute
* We will apply [Remote] attribute in the Email property of RegisterDto class 
* 
* Test at Register page, in email input form, type registered email, then press tab
* Error notice will appear below the input form
* 
* Look at: AccountController.cs, RegisterDto.cs
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