using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* HTTPS
* 
* If you enable the https in your application, by default, all the requests and responses will be automatically gets
* encrypted so that any information that is being shared between client and server will be secured.
* 
* Add middleware UseHttpsRedirection() to enable HTTPS
* Add middleware UseHsts() to enforce the browser to use HTTPS
* 
* In launchSettings.json, we must to change applicationUrl since we use HTTPS
* Change the protocol to https -> "https://localhost:5204"
* 
* Look at: Program.cs, launchSettings.json
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

// force to use Strict-Transport-Security header
app.UseHsts();
// notice this, notif the browser to use HTTPS, transport level security established between client & server
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