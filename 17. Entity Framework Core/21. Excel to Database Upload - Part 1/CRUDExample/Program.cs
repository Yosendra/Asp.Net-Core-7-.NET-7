using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Excel to Database Upload
 *   Add new country to the database through Excel file
 *   Add dependency Http in ServiceContract project in order to use IFormFile interface
 *   Add new contract on ICountryService and it implementation -> UploadCountryFromExcelFile(IFormFile formFile)
 * 
 *   Look at: CountryService.cs at UploadCountryFromExcelFile()
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
