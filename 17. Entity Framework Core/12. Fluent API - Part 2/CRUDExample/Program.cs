using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;

/* Fluent API
 *   is a set of predefined methods in entity framework that allows you to specifgy more details
 *   on the column data type that describe the table structure.
 *   
 *   the example is at PersonsDbContext at modelBuilder parameter inside OnModelCreating() we are accessing Entity() and ToTable() methods
 *   
 *   Like always Add-Migration, then Update-Database
 *   
 *   Look at: PersonsDbContext
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
