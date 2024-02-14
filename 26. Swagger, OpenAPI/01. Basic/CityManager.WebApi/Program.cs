
/* What is Swagger?
* 
* Swagger is a set of open-source tools that help developers to generate interactive UI to document and test RESTful services.
* Swagger is a set tools to implement Open API.
* 
* Open API 	             -> specification that defines how to write API specification in JSON
* Swagger  	             -> set of tools to generate UI to document & test RESTful services
* Swashbuckle.AspNetCore -> framework that makes it easy to use swagger in asp.net core
* 
* Install package 
* • Microsoft.AspNetCore.OpenApi
* • Swashbuckle.AspNetCore
* 
* Add Swagger services to ServiceContainer in Program.cs
* AddEndpointsApiExplorer() - it enables swagger to read metadata (HTTP method, URL, attributes, etc) of endpoints (Web API action methods)
* AddSwaggerGen() - it configures swagger to generate documentation for API's endpoints
* UseSwagger() - enables the endpoint for swagger.json file
* UseSwaggerUI() - create swagger UI for testing all Web API / endpoints / action methods
* 
* In launchSettings.json, at launchUrl key, assign "swagger" as its value
* 
* Run the application, we will swagger page to test our Web API
* 
* Look at: Program.cs
*/

using CityManager.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);


builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));

// Add Swagger Service
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();


var app = builder.Build();


app.UseHsts();
app.UseHttpsRedirection();

// Use Swagger UI
app.UseSwagger();
app.UseSwaggerUI();

app.UseAuthorization();
app.MapControllers();
app.Run();
