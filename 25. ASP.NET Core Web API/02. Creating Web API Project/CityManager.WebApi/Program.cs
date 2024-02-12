
/*
* Create project by choosing ASP.NET Web Api
* In Web API we do not have View, only Controller and Model
* 
* Apply UseHsts() middleware
* 
* Notice at launchSettings.json, at applicationUrl and launchUrl key
* applicationUrl : https://localhost:7087
* launchUrl      : weatherforecast
* So the url for browser to make a request to Web API is https://localhost:7087/weatherforecast
* 
* Run the application, and look at url
* 
* Look at: launchSettings.json, Program.cs
*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.

builder.Services.AddControllers();  // notice this, it without View

var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHsts();                      // enforce to use https
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
