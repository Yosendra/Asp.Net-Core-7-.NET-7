using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Generate Excel Files
 *   Install EPPlus package at Service project
 *   App some key in appsettings.json for non-commercial use
 *   Add new method in PersonService contract and implementation -> GetPersonExcel()
 *   Add new endpoint in PersonController -> PersonExcel()
 * 
 *   Look at: PersonService, PersonController, Index page for Person (adding hyperlink to download excel file)
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDbContext<PersonsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddServices()
    .AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

RotativaConfiguration.Setup("wwwroot");
app.UseStaticFiles();
app.MapControllers();
app.Run();
