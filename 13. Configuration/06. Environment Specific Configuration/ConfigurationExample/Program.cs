using ConfigurationExample;

// Environment Specific Configuration
//
// Order of Precendece of Configuration Sources
// 1. appsetting.json
// 2. appsetting.[environment].json ---> appsetting.Development.json
//                                       appsetting.Staging.json
//                                       appsetting.Production.json
// 3. User Secrets (Secret Manager)
// 4. Environment Variables
// 5. Command Line Arguments
//
// The latest value override the earlier value base on this sequence
// for example in appsetting.json there is x = 1, then in appsetting.Development.json there is also x = 3.
// Then x will take the value 3 from appsetting.Development.json
//
// case: you want to seperate your configuration settings between your development machine
//       and production machine like you want to use different database connection string
//       for each one
//
// Look at: launchSettings.json, appsettings.json, appsettings.Development.json

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
