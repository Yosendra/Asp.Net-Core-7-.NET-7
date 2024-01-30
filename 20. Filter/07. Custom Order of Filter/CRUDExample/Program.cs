using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* 
  Before we have learnt the default order execution of Filter, from broader scope to narrower
  We can customize the filter order execution, by assigning order number in "Order" property of the filter
  Filter with the lowest order number run first.
  
  Filter with lowest order number run first.
  If the order number is same for two or more filters, the filter with broader scope will execute first.
  Ther order of "after filter method" is the reverse of order of "before filter method"

  Example we want the filter execution sequence like this:
    Global -> Method -> Controller
    
    1. For global, in Program.cs, don't have any changes since we want it run first. (Start with 0)
    2. For method filter, we assign the "Order" property to 1. (in Index() method in PersonController)
    3. For controller filter, we assign the "Order" property to 2. (in PersonController)

  We can test the order execution by putting breakpoint at ResponseHeaderActionFilter contructor, then see the Key value field when application starting up
  
Look at: PersonController.cs, Program.cs
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