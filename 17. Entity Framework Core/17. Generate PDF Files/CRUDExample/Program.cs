using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Using 'Rotativa' as 3rd party package to generate PDF file (Install through NuGet Package Manager)
 * 
 *   Look at: PersonPDF.cshtml as the base view of our PDF page
 *            PersonController add action for generating the PDF, in this case it is PersonPDF() action in PersonController
 *            In order to Rotativa able to work, we need exe file wkhtmltopdf.exe placed in wwwroot folder
 *            Configure the wkhtmltopdf.exe path for Rotativa
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDbContext<PersonsDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddServices()
    .AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

RotativaConfiguration.Setup("wwwroot"); // notice this
app.UseStaticFiles();
app.MapControllers();
app.Run();
