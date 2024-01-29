using CRUDExample.Bootstrap;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Type Filter - with Argument

  When it runs                  You can supply an array of arguments that will be supplied as constructor arguments of the filter class.
  
  How to send arguments         [TypeFilter(typeof(_FilterClassName_), Arguments = new object[] { arg1, arg2 })]
  in controller                 public IActionResult ActionMethod()
                                {
                                  ....
                                }

  How to receive arguments      public _FilterClassName_(IService service, type param1, type param2)
  in filter's constructor       {
                                  ....
                                }
  
  We add new ActionFilter that is ResponseHeaderActionFilter class.
  This filter responsible to add additional key-value pair for response header.

  The filter receive argument when getting invoked.
  At PersonController we use the filter at the same Index() action method also giving the filter's arguments

  ResponseHeaderActionFilter can be reused to other action method.

  This whole thing is called "Parameterized Action Filter"

  We place two action filter in same action method, the question now is which one execute first?

Look at: ResponseHeaderActionFilter.cs, PersonController.cs
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
    .AddControllersWithViews();

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