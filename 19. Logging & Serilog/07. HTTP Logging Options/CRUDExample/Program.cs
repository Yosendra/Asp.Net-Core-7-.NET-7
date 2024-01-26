using CRUDExample.Bootstrap;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* HTTPLoggingFields enum
    RequestMethod                       Method of request. Eg: GET
    RequestPath                         Path of request. Eg: /home/index
    RequestProtocol                     Protocol of request. Eg: HTTP/1.1
    RequestScheme                       Protocol Scheme of request: Eg: http
    RequestQuery                        Query string Scheme of request. Eg: ?id=1
    RequestHeaders                      Headers of request. Eg: Connection: keep-alive
    RequestPropertiesAndHeaders         Includes all of above (default)
    RequestBody                         Entire request body. [has performanse drawback, not recommended]
    Request                             Includes all of above
    
    ResponseStatusCode                  Status code of response. Eg: 200
    ResponseHeaders                     Headers of response. Eg: Content-Length: 20
    ResponsePropertiesAndHeaders        Includes all of above (default)
    ResponseBody                        Entire response body. [has performanse drawback, not recommended]
    Response                            Includes all of above
    
    All                                 Includes all from request and response


    We add HTTP logging service in Dependency Container

Look at: 
*/

var builder = WebApplication.CreateBuilder(args);

builder.Host.ConfigureLogging(loggingProvider =>
{
    loggingProvider
        .ClearProviders()
        .AddConsole()
        .AddDebug()
        .AddEventLog();
});

builder.Services
    .AddHttpLogging(options =>  // Add HTTP logging into Dependency Container also define the logging field
    {
        options.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
    })
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddRepositories()
    .AddServices()
    .AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

app.UseHttpLogging();   // enable HTTP Logging

//app.Logger.LogDebug("debug-message");
//app.Logger.LogInformation("information-message");
//app.Logger.LogWarning("warning-message");
//app.Logger.LogError("error-message");
//app.Logger.LogCritical("critical-message");

if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");

app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program {}