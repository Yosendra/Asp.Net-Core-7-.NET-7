using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;

/* Changes in Table Structure
 *   case: we want to add a new column in Person table that is "TIN" or Text Identification Number
 *   
 *   Add new property at Person.cs: public string? TIN { get; set; }
 *   Go to Package Manager Console, then add migration
 *   Migration generating the new column in the model, waiting to be updated by Update-Database command
 *   
 *   Look at: Person.cs, we add TIN property
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
