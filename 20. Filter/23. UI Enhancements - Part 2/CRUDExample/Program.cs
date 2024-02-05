using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* UI Enchancement
* 
* Enhance the css layout and html template for the layout 
* 
* Look at: _Layout.cshtml, Index.cshtml view of PersonController
* 
* Continue enchance the view
* Adding the else condition at PersonListActionFilter for ViewBag
* 
* Look at: Create.cshtml, Edit.cshtml, Delete.cshtml view of PersonController
*          UploadFromExcel.cshtml view of CountryController
* 
*/

var builder = WebApplication.CreateBuilder(args);
builder.Host.UseSerilog((HostBuilderContext context, IServiceProvider service, LoggerConfiguration loggerConfiguration) =>
{
    loggerConfiguration
        .ReadFrom.Configuration(context.Configuration)
        .ReadFrom.Services(service);
});

builder.Services.AddTransient<PersonListActionFilter>();
builder.Services.AddTransient<ResponseHeaderActionFilter>();
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
    var logger = builder.Services
        .BuildServiceProvider()
        .GetRequiredService<ILogger<ResponseHeaderActionFilter>>();
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