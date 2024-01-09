using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;

/* Add DbContext as Service
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
