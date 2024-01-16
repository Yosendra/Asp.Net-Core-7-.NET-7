using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;

/* Seed Data
 *   Adds initial data (initial rows) in tables when database is newly created.
 *    in DbContext: modelBuilder.Entity<ModelClass>().HasData(entityObject);
 *   
 *   Look at: PersonsDbContext.cs
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
