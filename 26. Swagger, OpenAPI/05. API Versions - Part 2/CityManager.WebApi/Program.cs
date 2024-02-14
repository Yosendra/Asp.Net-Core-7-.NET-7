
/* API Versions - Part 2
*           
* The API version reader consist of three options:
* • QueryStringApiVersionReader
* • HeaderApiVersionReader
* • UrlSegmentApiVersionReader
* 
* If we are using "QueryStringApiVersionReader", we need to assign instance of "QueryStringApiVersionReader" on config.ApiVersionReader
* On the route attribute of the CustomContollerBase, remove the version from path template
* Now the path become like this "/api/city?api-version=1.0", notice the "api-version=1.0" in the query string
* 
* Specify the default API version when the API version is not mentioned 
* Configure "config.DefaultApiVersion" and "config.AssumeDefaultVersionWhenUnspecified" at AddApiVersioning()
* Test by accessing "/api/city" without "api-version=1.0" in query string. The API v1 is accessed
* 
* If we are using "HeaderApiVersionReader", we need to assign instance of "HeaderApiVersionReader" on config.ApiVersionReader
* We need to add "api-version" key in the header request. Eg: "api-version: 1.0"
* To test it we can use POSTMAN to add the key before sending the request. Eg: "api-version: 1.0"
* 
* Look at: Program.cs, CustomContollerBase.cs
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

builder.Services.AddApiVersioning(config => {
        /*** Read version number from request url at "apiVersion" constraint ***/
        //config.ApiVersionReader = new UrlSegmentApiVersionReader();

        /*** Read version number from request query string called "api-version" ***/
        //config.ApiVersionReader = new QueryStringApiVersionReader();
    
        /*** Read version number from request header "api-version" key. Eg: "api-version: 1.0" ***/
        config.ApiVersionReader = new HeaderApiVersionReader("api-version");

        config.DefaultApiVersion = new ApiVersion(1, 0);    // define the default API version used if not mentioned the version
        config.AssumeDefaultVersionWhenUnspecified = true;
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
