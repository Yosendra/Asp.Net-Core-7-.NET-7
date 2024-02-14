
/* API Versions - Part 1
* 
* API Versioning is the practice of transparently managing changes to your API
* where the client request a specific version of API and the server executes the same version of the API code
*           
*           HTTP request to API v1
*           ---------------------->           --->   Asp.Net Core Web API
*           <----------------------                  Controller v1
* Client                               Server
*           HTTP request to API v2
*           ---------------------->           --->   Asp.Net Core Web API
*           <----------------------                  Controller v2
*           
* Delete TestController
* Add sub-folder "v1" in Controllers folder
* Move CityController to that folder
* Add sub-folder "v2" in Controllers folder
* Copy and Paste CityController to that folder also
* 
* Our requirement, in v2, we want to return city's name only without id
* No enpoint for PUT, DELETE, and POST
* 
* Enable API versioning in Swagger
* Install these packages: 
* • "Microsoft.AspNetCore.Mvc.Versioning"
* • "Microsoft.AspNetCore.Mvc.Versioning.ApiExplorer"
* 
* Enable the API versioning through ServiceContainer by adding "AddApiVersioning()"
* In CustomControllerBase, modify the route template, add the version 
* To define the version controller, use [ApiVersion] attribute on each controller CityController (v1), CityController (v2)
* 
* Test : Run the application, then request to this path "/api/v1/city" and "/api/v2/city"
* 
* Look at: CityController.cs (v2), CityController.cs (v1), Program.cs, CustomControllerBase.cs
*/

using CityManager.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options => {
        options.Filters.Add(new ProducesAttribute("application/json"));
        options.Filters.Add(new ConsumesAttribute("application/json"));
    })
    .AddXmlSerializerFormatters();

builder.Services.AddApiVersioning(config => {    // enable API versioning
        config.ApiVersionReader = new UrlSegmentApiVersionReader();
    });

builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options => options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml")));
var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI();
app.UseAuthorization();
app.MapControllers();
app.Run();
