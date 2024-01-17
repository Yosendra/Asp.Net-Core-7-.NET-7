using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;

/* Code-First Migrations
 *   Create or updates database based on the changes made in the model.
 *   
 *         ---> Migration 1 ---> 
 *   Model ---> Migration 2 ---> DB
 *         ---> Migration 3 ---> 
 *   
 *   To run the migration we need to put a command in "Package Manager Console"
 *     At main menu go to Tools -> NuGet Package Manager -> Package Manager Console
 *     Change the default project to: Entities
 *       Command: Add-Migration [migration_name]      ex: Add-Migration Initial
 *   
 *   (Currently we are using .NET 6, so use the version 6.x at the package, make all packagages version equal.
 *   In this solution we use version 6.0.5 to EntityFrameworkCore sqlserver, design, tools)
 *   To be able to use migration, we need to install package Microsoft.EntityFrameworkCore.Tools
 *     Put this command on Package Manager Console of 'Entity' project: Install-Package Microsoft.EntityFrameworkCore.Tools
 *      or
 *     You can install through 'Manage NuGet Package' 
 *   
 *   To be able to use Microsoft.EntityFrameworkCore.Tools, we need to install package Microsoft.EntityFrameworkCore.Design at UI project (in this case is CRUDExample project)
 *     Put this command on Package Manager Console of 'Entity' project: Install-Package Microsoft.EntityFrameworkCore.Design
 *      or
 *     You can install through 'Manage NuGet Package'
 *   
 *   Add contructor at PersonsDbContext to pass the options
 *   
 *   After you run the command Add-Migration Initial, it will generate Migration folder and its containing files
 *   
 *   After Migration generated, to apply affect the changes into the actual DB run the command "Update-Database -verbose" at Package Manager Console
 *   
 *   Now you may check the table's data in SQL Server Object Explorer
 *     
 *   Look at: PersonDbContext.cs we are adding constructor to use the option we define in AddDbContext at Program.cs
 *            Migration folder at Entities project (genereted after running the command Add-Migration Initial)
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDbContext<PersonsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddServices()
    .AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();
app.UseStaticFiles();
app.MapControllers();
app.Run();
