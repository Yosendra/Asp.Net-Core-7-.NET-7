
/* Documentation Comments
* 
* We can add description of an endpoint through XML comment
* It will reflect to Swagger UI, giving description of use of that certain endpoint
* 
* Right click on project, choose build, check in checkbox "Generate a file containing API documentation"
* Below option, give the name of XML documentation file, for example we give "api.xml"
* Build the project to generate api.xml
* 
* Include api.xml as XML documentation to swagger in Program.cs
* 
* We can run the application and see now there is description on endpoint method for GetCity (GET)
* 
* Look at: CityController.cs, api.xml, Program.cs
*/

using CityManager.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml")));   // include XML comment to Swagger 
var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
