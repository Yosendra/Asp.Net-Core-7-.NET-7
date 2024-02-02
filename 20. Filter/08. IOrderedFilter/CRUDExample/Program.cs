using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* 
  As we have seen already we can set the "Order" property value in the [TypeFilter] attribute,
  but it is not possible for global-filter. To overcome this the alternative way is to set the order
  by using the predefined interface "IOrderedFilter". It is more preferred way.
  
  We try to implement this interface at ResponseHeaderActionFilter
  Assign the Order property through constructor invocation of ResponseHeaderActionFilter in PersonController->Index, PersonController, and global-filter at Program.cs
  
  We can test this by putting breakpoint in "OnActionExecuting" of ResponseHeaderActionFilter and observe the Key, Value, and Order property

  Notice this is synchronous filter
  
Look at: ResponseHeaderActionFilter.cs, Program.cs, PersonController.cs
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

        //options.Filters.Add(new ResponseHeaderActionFilter(logger, "Key-From-Global", "Value-From-Global"));         // Before, global-level
        options.Filters.Add(new ResponseHeaderActionFilter(logger, "Key-From-Global", "Value-From-Global", 2));        // Add the the "order" argument
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