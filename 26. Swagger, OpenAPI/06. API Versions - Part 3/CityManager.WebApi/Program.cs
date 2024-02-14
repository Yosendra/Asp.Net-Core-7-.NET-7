
/* API Versions - Part 3
*           
* There is still error when in the Swagger when we runnning the apps
* We need to configure to generate Swagger documentation fot both API version we have created (v1, v2) inside AddSwaggerGen()
* 
* After that, we need to enable two endpoint for both Swagger version in UseSwaggerUI() middleware
* After that, add AddVersionedApiExplorer() in ServiceContainer. it needed for Swagger to know how to substitute the version number.
* 
* Look at: Program.cs
*/

using CityManager.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Versioning;
using Microsoft.EntityFrameworkCore;
using Microsoft.OpenApi.Models;

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllers(options =>
{
    options.Filters.Add(new ProducesAttribute("application/json"));
    options.Filters.Add(new ConsumesAttribute("application/json"));
})
.AddXmlSerializerFormatters();
builder.Services.AddApiVersioning(config => 
{
    config.ApiVersionReader = new UrlSegmentApiVersionReader();
    config.DefaultApiVersion = new ApiVersion(1, 0);
    config.AssumeDefaultVersionWhenUnspecified = true;
});
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen(options =>
{
    options.IncludeXmlComments(Path.Combine(AppContext.BaseDirectory, "api.xml"));

    /*** Generate Swagger Documentation for both version ***/
    options.SwaggerDoc("v1", new OpenApiInfo()
    {
        Title = "City Web API",
        Version = "1.0",
    });
    options.SwaggerDoc("v2", new OpenApiInfo()
    {
        Title = "City Web API",
        Version = "2.0",
    });
});
builder.Services.AddVersionedApiExplorer(options =>
{
    options.GroupNameFormat = "'v'V";
    options.SubstituteApiVersionInUrl = true;
});
var app = builder.Build();

app.UseHsts();
app.UseHttpsRedirection();
app.UseSwagger();
app.UseSwaggerUI(options =>
{
    // enable two endpoint for both Swagger document version
    options.SwaggerEndpoint("/swagger/v1/swagger.json", "1.0");
    options.SwaggerEndpoint("/swagger/v2/swagger.json", "2.0");
});
app.UseAuthorization();
app.MapControllers();
app.Run();
