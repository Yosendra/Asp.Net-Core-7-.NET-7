using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Mock Repository
 *   
 *   Test still not complete in PersonTest
 *   
 *   Look at: PersonServiceTest.cs at AddPerson_Success_ReturnPersonResponseWithId()
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
