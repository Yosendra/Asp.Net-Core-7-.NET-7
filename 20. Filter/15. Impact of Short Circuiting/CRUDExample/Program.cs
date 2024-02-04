using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Impact of Short-Circuiting
* 
* Short-circuiting the filters
*
* Authorization filter
*   context.Result = some_action_result
*   • Bypass all filters, action method execution & result execution
* 
* Resource filter
*   context.Result = some_action_result
*   • Bypass model binding, action filter, action method, result execution & result filter
*   • Resource filter "Executed" method run with context.Cancelled = true
*   
* Action filter
*   context.Result = some_action_result
*   • Bypass only action method execution
*   • Other action filter "Executed" methods with context.Cancelled = true; and also all result filters, resource filter run normally
*   
* Exception filter
*   context.Result = some_action_result
*   [or]
*   context.ExceptionHandled = true
*   • Bypass result execution & result filter
*   • All resource filter "Executed" method run
*  
* Result filter
*   context.Cancelled = true
*   • Bypass only result execution.
*   • Other result filter "Executed" methods & all resource filter run normally
* 
* Look at: 
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
    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
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