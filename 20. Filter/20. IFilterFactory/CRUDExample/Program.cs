using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* IFilterFactory
* 
* case: We want our ResponseHeaderActionFilter attribute to also support contructor DI. We can do that with IFilterFactory
* 
* We modify our ResponseHeaderActionFilter
* Add new class ResponseHeaderFilterFactoryAttribute in the same namespace as ResponseHeaderActionFilter
* Make ResponseHeaderFilterFactoryAttribute inherits from Attribute class & implements IFilterFactory interface
* Long explanation, just see the code in these files listed below
* 
* Best practice in real world project, avoid using action filter attribute or result filter attribute
* Either you can use IFilterFactory or TypeFilter or ServiceFilter
* FilterAttribute is not recomended because it is not supporting constructor injection
* That's why if you still want to use FilterAttribute then use the IFilterFactory
* 
* Look at: ResponseHeaderActionFilter.cs, ResponseHeaderFilterFactoryAttribute class, PersonController.cs, Program.cs
*/

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(service);
});

builder.Services.AddTransient<PersonListActionFilter>();
builder.Services.AddTransient<ResponseHeaderActionFilter>();            // don't forget to register ResponseHeaderActionFilter into ServiceCollection
builder.Services.AddHttpLogging(configureOptions =>
{
    configureOptions.LoggingFields = HttpLoggingFields.RequestProperties | HttpLoggingFields.ResponsePropertiesAndHeaders;
});
builder.Services.AddDbContext<ApplicationDbContext>(options =>
{
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
});
builder.Services.AddRepositories();
builder.Services.AddServices();
builder.Services.AddControllersWithViews(options => 
{
    //options.Filters.Add(new ResponseHeaderActionFilter("Key-From-Global", "Value-From-Global", 2));

    var logger = builder.Services.BuildServiceProvider().GetRequiredService<ILogger<ResponseHeaderActionFilter>>();         // notice this
    options.Filters.Add(new ResponseHeaderActionFilter(logger)
    {
        Key = "Key-From-Global",
        Value = "Value-From-Global",
        Order = 2,
    });
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