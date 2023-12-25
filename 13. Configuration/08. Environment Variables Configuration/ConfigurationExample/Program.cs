using ConfigurationExample;

// Environment Variables Configuration
//
// Set configuration as Environment Variables
//   in "Windows PowerShell" / "Developer PowerShell in VS"
//     $Env:ParentKey__ChildKey="value" -> in PowerShell
//     set ParentKey__ChildKey="value"  -> in Command Prompt
//
//     example: $Env:WeatherApi__ClientId="ClientID from environment variables"
//              $Env:WeatherApi__ClientSecret="ClientSecret from environment variables"
//
//              set WeatherApi__ClientId="ClientID from environment variables"
//              set WeatherApi__ClientSecret="ClientSecret from environment variables"
//
//   to simulate -> dotnet run --no-launch-profile
//   then in the browser go to this path -> http://localhost:5000/
//
//   it is one of the most secured way of setting-up sensitive values in configuration
//   __ (underscore underscore) is the seperator between parent key and child key

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.Configure<WeatherApiOptions>(
    builder.Configuration.GetSection("WeatherApi"));

var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.MapControllers();
app.Run();
