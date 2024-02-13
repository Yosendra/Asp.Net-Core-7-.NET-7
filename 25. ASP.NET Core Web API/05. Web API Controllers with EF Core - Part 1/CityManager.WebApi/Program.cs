
/* Web API Controllers with EF Core
* 
* Scafold new Controller by right-click on Controller folder, choose add Controller
* then choose "API Controller with action, using Entity Framework"
* 
* Insight: Web API only take json data, and return json data (if json)
*          So, in POSTMAN body, choose raw (not form-data, if choose this, there will be error unsupported data), then choose JSON
*          
*          {
*            "Id": "11525A00-EA33-45AE-993C-3E654FCE0EF2",
*            "Name": "Rio De Janairo"
*          }
*          
*          As per REST standard, we should use these HTTP methods to request these following operation:
*          • GET to get data
*          • POST to create data
*          • PUT to update data
*          • DELETE to delete data
*          
* Test: try to make above operation through POSTMAN application
*          
* Look at: CityController.cs
*/

using CityManager.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
