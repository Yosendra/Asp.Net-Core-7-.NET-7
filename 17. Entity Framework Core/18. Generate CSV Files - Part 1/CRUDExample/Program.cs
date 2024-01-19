using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Using 'CsvHelper' as 3rd party package to generate CSV file (Install through NuGet Package Manager)
 * 
 *   Look at: IPersonService we add new contract "Task<MemoryStream> GetPersonCSV()" then implement this in PersonService
 *            Install CsvHelper package at Services project
 *            Implement GetPersonCSV() at PersonService
 *            Add new endpoint for converting CSV file at PersonController, in this case it is PersonCSV()
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
