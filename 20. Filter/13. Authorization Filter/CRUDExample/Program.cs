using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Authorization Filter (IAuthorizationFilter, IAsyncAuthorizationFilter)
* When it runs?                 Runs before any other filters in the filter pipeline
* 
* OnAuthorize method            Determine whether the user is authorized for the request
*                               Short-Circuit the pipeline if the request is NOT authorized
*                               Don't throw exception in OnAuthorize method, as they will not be handled by exception filters
*                               
* Cookie         : are stored in the browser but the server can send cookies to the browser first, the browser store the cookie in memory
*                  and that cookie will be automatically submitted for all the subsequent request to the same domain.
* Authentication : checks whether the user is logged in or not
* Authorization  : checks whether the particular user has permission to access the particular resource or not
* 
* case: We want to use athentication functionailty with authorization filter
*       Create our custom authorization filter TokenAuthorizationFilter
*       We apply this filter in Edit (POST) in PersonController
*       
*       We create TokenResultFilter for generating token
*       We apply this filter in Edit (GET) in PersonController
*       
*       We can cross check if the cookie really sent to browser by inspecting element in the browser go to application tab,
*       in storage category look at cookie
*       
*       Now we can test this by go to Edit person page, then make a request to the edit person. 
*       Put breakpoint at custom Authorization filter and Result filter we create before
*       
* Look at: TokenAuthorizationFilter.cs, TokenResultFilter, PersonController.cs
*/

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
    {
        loggerConfiguration
            .ReadFrom.Configuration(context.Configuration)
            .ReadFrom.Services(service);
    });
builder.Services
    .AddHttpLogging(configureOptions => 
    {
        configureOptions.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
    })
    .AddDbContext<ApplicationDbContext>(options => 
    {
        options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
    })
    .AddRepositories()
    .AddServices()
    .AddControllersWithViews(options =>
    {
        var logger = builder.Services
            .BuildServiceProvider()
            .GetRequiredService<ILogger<ResponseHeaderActionFilter>>();

        options.Filters.Add(new ResponseHeaderActionFilter(logger, "Key-From-Global", "Value-From-Global", 2));
    });

var app = builder.Build();
app.UseSerilogRequestLogging();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
app.UseHttpLogging();
if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");
app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program {}