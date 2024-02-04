using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* ServiceFilter [vs] TypeFilter Attribute
* 
* Common purpose -> both are used to apply a filter at controller or action method
* 
* Type Filter    -> [TypeFilter(typeof(FilterClassName), Arguments = new object[] { false, })]
* • Can supply arguments to filter
* 
* Service Filter -> [TypeFilter(typeof(FilterClassName))]
* • Cannot supply arguments to filter
* 
* You can use either of these interchangeably
* But one thing, in order to be able to use ServiceFilter, we need to register the filter class in IoC container
* 
* Look at: Program.cs
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

builder.Services.AddTransient<PersonListActionFilter>();                // Register PersonListActionFilter to IoC container

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