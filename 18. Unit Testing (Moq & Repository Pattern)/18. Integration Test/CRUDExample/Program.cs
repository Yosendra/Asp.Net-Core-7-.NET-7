using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Integration Test
 *   Unit Testing         : Unit Test -> Controller -> Service Mock 
 *   Integration Testing  : Intergration Test -> HttpClient Request -> Controller -> Service -> Repository -> DbContext -> Database
 *   
 *   In Unit Test, we focus on single component. Focus only on Controller or Service without involving its underlying or higher level component
 *   In Integration Test, we focus on the overall functionality of our application. We create object for sending HTTP request to simulate real browser request.
 *   
 *   In order to be able running our application for Integration Test, we create a custom web application factory. So we need not to run our application
 *   in the browser for testing purpose.
 *   
 *   In Program.cs we create partial class for the Program class itself. With this we can access Program class with its automatically generated content
 *   
 *   Add this to CRUDExample.csproj, to give permission to access the automatically generated Program class to outside project (in this case CRUDTest)
 *   <ItemGroup>
 *       <InternalsVisibleTo Include="CRUDTests" />
 *   </ItemGroup>
 *   
 *   Install package for using WebApplicationFactory class in CRUDTest
 *     Microsoft.AspNetCore.Mvc.Testing
 *   
 *   In WebApplicationFactory we implement some logic for running Program for Integration Test including configure the needed service 
 *   
 *   EFCore In Memory Database Provider
 *          ---> Microsoft.EntityFrameworkCore.InMemory ---> Collection
 *   DbContext
 *          ---> Microsoft.EntityFrameworkCore.SqlServer ---> SQL Server
 *          
 *   Install package Microsoft.EntityFrameworkCore.InMemory in CRUDTest
 *   
 *   In Program.cs we need to exclude Rotativa service (for generating pdf) in our integration testing
 *   
 *   Run the integration test for PersonControllerIntegrationTest
 *   
 *   Look at: PersonControllerIntegrationTest.cs, CustomWebApplicationFactory.cs
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddRepositories()
    .AddServices()
    .AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

if (!app.Environment.IsEnvironment("IntegrationTest"))
    RotativaConfiguration.Setup("wwwroot");

app.UseStaticFiles();
app.MapControllers();
app.Run();

public partial class Program {} // make the auto-generated program accessible programmatically for developer