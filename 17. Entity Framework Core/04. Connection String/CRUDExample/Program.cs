using Entities;
using Microsoft.EntityFrameworkCore;
using ServiceContracts;
using Services;

/* Connection String
 *   is string value that specifies the details of the database that you would like to connect.
 *   
 *   To get this connection string, go to View -> SQL Server Object Explorer. Create our db in our local db instance, 
 *   then prperties on that db, check the connection string.
 *   
 *   We put this connection string in config file appsetting.json -> ConnectionStrings:DefaultConnection
 *   Data Source=(localdb)\MSSQLLocalDB;Initial Catalog=PersonsDatabase;Integrated Security=True;Connect Timeout=30;Encrypt=False;Trust Server Certificate=False;Application Intent=ReadWrite;Multi Subnet Failover=False
 * 
 * Look at appsetting.json -> ConnectionStrings key,
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.AddSingleton<ICountryService, CountryService>();
builder.Services.AddSingleton<IPersonService, PersonService>();

builder.Services.AddDbContext<PersonsDbContext>(options =>
{
    // Put the connection string here
    options.UseSqlServer(builder.Configuration["ConnectionStrings:DefaultConnection"]); // can be written like this
    // or
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")); // or like this
});

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.MapControllers();
app.Run();
