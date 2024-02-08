using CRUDExample;
using CRUDExample.Middleware;
using Rotativa.AspNetCore;
using Serilog;

/* Test
* 
* Add 3 new project in Test folder: 
*   ContactManager.ServiceTests
*   ContactManager.ControllerTests
*   ContactManager.IntegrationTests
* 
* Double click on UI project, then add this:
*   <ItemGroup>
*     <InternalsVisibleTo Include="ContactManager.IntegrationTests" />
*   </ItemGroup>
* To enable auto generated Program class to able be accessed in ContactManager.IntegrationTests project
* 
* Install package in those 3 test projects
*   FluentAssertions
*   AutoFixture
*   Moq
* 
* Instal these pacakage in "ContactManager.IntegrationTests" project
*   Microsoft.AspNetCore.Mvc.Testing
*   Microsoft.EntityFrameworkCore.InMemory
*   Fizzler
*   Fizzler.Systems.HtmlAgilityPack
* 
* Add project reference to UI project from ControllerTests project
* Add project reference to UI project from IntergrationTests project
* Add project reference to Core project from ServiceTests project
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
builder.Services.ConfigureServices(builder.Configuration);


var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
else
{
    app.UseExceptionHandler("/Error");
    app.UseExceptionHandlingMiddleware();
}
app.UseSerilogRequestLogging();
app.UseHttpLogging();
if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");
app.UseStaticFiles();
app.UseRouting();
app.MapControllers();
app.Run();

public partial class Program {}