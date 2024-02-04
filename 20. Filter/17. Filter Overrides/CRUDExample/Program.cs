using CRUDExample.Bootstrap;
using CRUDExample.Filters.ActionFilters;
using Entities;
using Microsoft.AspNetCore.HttpLogging;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;
using Serilog;

/* Filter Overrides
* 
* We put the PersonAlwaysRunResultFilter at controller level
* case: We want to not run the PersonAlwaysRunResultFilter in index action method at PersonController
* 
* Create SkipFilter, inherits Attribute, implements IFilterMetadata.
*   Whenever a class inherits from Attribute, that class can be treated as attribute in the class or action method
*   Whenever a class implements IFilterMetadata, that class can act as a filter
* In PersonAlwaysRunResultFilter, put the logic to skip this filter. 
* Apply the SkipFilter attribute in Index action method in PersonController
* Put breakpoint at the logic for skipping the filter at PersonAlwaysRunResultFilter
* Access the Index of PersonController
* 
* Look at: PersonAlwaysRunResultFilter.cs, SkipFilter.cs, PersonController.cs
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