using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;

/* DbContext & DbSet
 * 
 * Custom DbContext class                                    SQL Server
 * 
 * public class CustomDbContext : DbContext -----------------> SQL Database
 * {
 *   public DbSet<ModelClass1> DbSet1 { get; set; } -----------> Table 1
 *   public DbSet<ModelClass2> DbSet2 { get; set; } -----------> Table 2
 * }
 * 
 * DbContext -> An instance of DbContext responsible to hold a set of DbSet and represent a connection with database.
 * DbSet     -> Represent a single database's table, each column is represented as a model property.
 * 
 * Add DbContext as Service
 * 
 * in Program.cs
 * builder.Services.AddDbContext<DbContextClassName>(options => { options.UseSqlServer(); });
 * 
 * Look at Country.cs, Person.cs entity, PersonsDbContext.cs, Program.cs
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ICountryService, CountryService>();
builder.Services.AddSingleton<IPersonService, PersonService>();

/* Inject DbContext as a service.
 * With this, we can use instance of PersonsDbContext anywhere, either in controller or service class
 */
builder.Services.AddDbContext<PersonsDbContext>(options =>
{
    /* because the concept of DbContext is common for all types of databases
     * such as MySql, Sqlite, etc. In order to use SqlServer as database connection 
     * we need to pass this lambda expression
     */
    options.UseSqlServer();
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.MapControllers();
app.Run();
