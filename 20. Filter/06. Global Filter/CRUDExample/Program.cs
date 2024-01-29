using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* 
  Global Filter
    are applied to all action methods of all controllers in the project.

  ----------------------------------------------------
  [Filter]  //global-level filter
  Asp.Net Core Project

    [Filter]  //class-level filter
    class Controller
    {
      [Filter]  //method-level filter
      ActionMethod
      {
        ...
      }
    }
  ----------------------------------------------------
  
  Try to demonstrate class-level (controller) filter at PersonController
  
  We can also implement filter in global-level through Program.cs
  Global Filter are applied to all action of all controllers in the project
  
  builder.Services.AddControllersWithViews(options => 
  {
    options.Filters.Add<FilterClassName>();  //add by type
    [or]
    options.Filters.Add(new FilterClassName()); //add filter instance
  })
  
  We add global-level filter through DependencyInjection
  So there will be 3 layer filter accessed for request come from path "/index", 
  Then we can observe the additional key-value pair in the response header
  
  The filter sequence for "/index" path : Global -> Controller -> Action        (for request)       rule: from broader to narrower scope
  The filter sequence for "/index" path : Action -> Controller -> Global        (for response)      rule: from narrower to broader scope
  
Look at: Program.cs, PersonController.cs
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
    .AddControllersWithViews(options =>     // notice this
    {
        //options.Filters.Add<ResponseHeaderActionFilter>();  // implement global-level filter, but cannot pass the argument for the constructor

        var logger = builder.Services
            .BuildServiceProvider()
            .GetRequiredService<ILogger<ResponseHeaderActionFilter>>(); // this to get the logger service object

        options.Filters.Add(new ResponseHeaderActionFilter(logger, "Key-From-Global", "Value-From-Global"));
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