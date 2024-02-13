
/* EntitytFrameworkCore with Web API
* 
* EntityFrameworkCore is light-weight, extensible and cross-platform framework for accessing database in .NET applications.
* It is the most-used database framework for Asp.Net Core Apps.
* 
* .NET Core App ---> EntityFrameworkCore ---> Database
* 
* Install these packages in WebApi project:
* • Microsoft.EntityFrameworkCore.SqlServer
* • Microsoft.EntityFrameworkCore.Tools
* 
* Create folder named "Models"
* Add City class into that folder
* 
* Create folder named "Entities"
* Add ApplicationDbContext class into that folder
* 
* Add new database through "SQL Server Object Explorer" name it "CityDatabase"
* Right click on the database, choose properties, then copy the ConnectionString
* Paste it in appsettings.json -> ConectionString -> Default
* 
* Register ApplicationDbContext to ServiceContainer in Program.cs
* 
* Open "Package Manager Console"
* Insert command "Add-Migration Initial"
* Then insert command "Update-Database"
* 
* Look at: City.cs, ApplicationDbContext.cs, appsettings.json, Program.cs
*/

using CityManager.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));  // Registering ApplicationDbContext
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
