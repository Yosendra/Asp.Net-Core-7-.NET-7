
/* Content Negotiation
* 
* Content negotiation is the process of selecting the approriate format or language of the content
* to be exchanged between the client (browser) and Web API.
* 
*                   HTTP request -> Content-Type: application/json
*                                   Accept: application/json
*                   --------------------------------------------->
* Browser (Client)                                                  Asp.Net Core (Server)
*                                                                   Web API Controller
*                   <---------------------------------------------
*                   HTTP response
*                   
* Add configuration to use content negotiation.
* Add ProducesAttribute filter in global scope in AddControllers() in Program.cs (response body type)
* Add ConsumesAttribute filter in global scope in AddControllers() in Program.cs (request body type)
* 
* So if any other client mentions content type as other than "application/json" in the request body
* that request would not be accepted and then the client get 415 Unsupported Media Type error response
* 
* We try to implement producing "application/xml" instead in certain endpoint by using [Produce] attribute in CityController
* 
* XML serialization is not active by default in asp.net core, we need enable it explicitly in ServiceContainer
* Add chain invoke AddXmlSerializerFormatters() after AddControllers()
* 
* Test: run the application, request to GetCity endpoint (get city list), take a look at the reponse body now in xml format 
* 
* Look at: Program.cs, CityController.cs
*/

using CityManager.WebApi.Entities;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

builder.Services
    .AddControllers(options =>
    {   // default content type of all action methods is "application/json" for response body
        options.Filters.Add(new ProducesAttribute("application/json"));
        // default content type of all action methods is "application/json" for request body
        options.Filters.Add(new ConsumesAttribute("application/json"));
    })
    .AddXmlSerializerFormatters();    // enable XML serialization service


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
