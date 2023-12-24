
// Environment in Launch Settings
//   look at "Properties/launchSettings.json"
// 
// Case: if the current environment is developer, you can enable the developer exception page
//       if the current environment is production, you can enable the custom error page which
//       user friendly but not for developer.
//
// IWebHostEnvironment
//
// EnvironmentName (property)
//   Gets or sets name of the environment. By default it reads the value from
//   either DOTNET_ENVIRONMENT or ASPNETCORE_ENVIRONMENT
//
// ContentRootPath (property)
//   Gets or sets absolute path of the application (project) folder
//
// IsDevelopment() (method)
//   Returns boolean true if the current environment name is "Development"
//
// IsStagin() (method)
//   Returns boolean true if the current environment name is "Staggin"
//
// IsProduction() (method)
//   Returns boolean true if the current environment name is "Production"
//
// IsEnvironment(string environmentName) (method)
//   Returns boolean true if the current environment name matches with specific environment

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();

// launchSettings.json
//
//  "environmentVariables": {
//      "ASPNETCORE_ENVIRONMENT": "Development"
//       }
//
// Change the "Development" to "Staging" or something else
// Read the current environment is on 'Development' or 'Stagging' 
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    // Enable and use the developer exception page
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.MapControllers();

app.Run();
