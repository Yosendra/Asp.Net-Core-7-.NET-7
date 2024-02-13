
/* ProblemDetails
* 
* We can return ProblemDetails object when there is some error
* Example we want to return ProblemDetails object when supplied id doesn't exist
*          
* Add [Required] attribute at Name property of City
* Migrate at Package Manager Console -> "Add-Migration CityNameRequired"
* Then -> "Update-Database"
* Now name at City table is mandatory field in database reflects to EntityFramework
* 
* For validation error (on Model Binding process) we return ValidationProblemDetails object
* We try to implement it in POST City, but actually it automatically done by ApiController
* We write it just for learning purpose
* 
* Look at: CityController.cs (GET & POST city),  
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
