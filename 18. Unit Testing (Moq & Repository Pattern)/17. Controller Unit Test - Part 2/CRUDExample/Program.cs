using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Controller Unit Test
 *   Runtime      :              Controller -> Service -> Repository -> DbContext  
 *   Unit Testing : Unit Test -> Controller -> Service Mock
 *   
 *   Add reference project to CRUDExample in CrudTest project
 *   
 *   Look at: PersonControllerTest.cs (enough)
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

RotativaConfiguration.Setup("wwwroot");
app.UseStaticFiles();
app.MapControllers();
app.Run();
