using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Filter Attribute Classes
* 
* We can write filter when applying it in shortcut and concise way, for example like writing attribute [ResponseHeaderFilter]
* It is possible by creating our custom filter class, then inheriting it with FilterAttribute class
* 
* IActionFilter vs ActionFilterAttribute
* 
* IActionFilter             public class FilterClassName : IActionFilter, IOrderedFilter 
*                           {
*                               // support constructor DI
*                           }
*                       
*                           Filter Interface:
*                           • IAuthorizationFilter          • IAsyncAuthorizationFilter
*                           • IResourceFilter               • IAsyncResourceFilter
*                           • IActionFilter                 • IAsyncActionFilter
*                           • IExceptionFilter              • IAsyncExceptionFilter
*                           • IResultFilter                 • IAsyncResultFilter
*                       
* ActionFilterAttribute     public class FilterClassName : ActionFilterAttribute
*                           {
*                               // doesn't support constructor DI
*                           }
*                           
*                           Fitler attribute:
*                           • ActionFilterAttribute
*                           • ExceptionFilterAttribute
*                           • ResultFilterAttribute
*                       
* We would like to modify ResponseHeaderActionFilter to inherit from ActionFilterAttribute instead of filter interface like before                      
* FilterAttribute doesn't support constructor DI, so delete ILogger in the constructor and field
* Apply ResponseHeaderActionFilter attribute we have modify in PersonController.cs and Program.cs
* 
* 
* Look at: ResponseHeaderActionFilter.cs, Program.cs, PersonController.cs
*/

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(service);
});
builder.Services.AddHttpLogging(configureOptions => 
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
    //var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();           // doesn't supposrt constructor DI
    //options.Filters.Add(new ResponseHeaderActionFilter(logger, "Key-From-Global", "Value-From-Global", 2));
    
    options.Filters.Add(new ResponseHeaderActionFilter("Key-From-Global", "Value-From-Global", 2));
});

builder.Services.AddTransient<PersonListActionFilter>();

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