using ConfigurationExample;

// Secret Manager
//
//   case: you have your own config which you keep push it to source control. Other developers
//         can see it. but you don't want to share it.
//         
// The 'secret manager' stores the user secrets (sensitive configuration data) in a seperate
// location on the developer machine.
//
// Source Code Repository       read         in UserSecretId.json
// App source code ----------------------->  {
//                                              "key": "value"
//                                           }
//
// Enable Secret Manager
//   in "Windows PoserShell" / "Developer PowerShell in VS"
//
//   COMMAND:
//    dotnet user-secrets init
//
//    dotnet user-secrets set "key" "value"
//      example: dotnet user-secrets set "WeatherApi:ClientId" "ClientID from user secrets"
//               dotnet user-secrets set "WeatherApi:ClientSecret" "ClientSecret from user secrets"
//
//    dotnet user-secrets list
//
//   Go to menu in VS : View -> Terminal
//   In the terminal got to the project folder, then you can run the command above
//   
//   <UserSecretsId> property added to the project, you can see it.
//   If you want to see the list key-value pair in user-secrets,
//   right click in the project then click 'Manage User Secrets' it saved in secrets.json in the its directory.
//   
//   It is important to setting launchSettings.json, the environment to "Development" to use this user secret
//    "environmentVariables": {
//          "ASPNETCORE_ENVIRONMENT": "Development"
//     }
//   Finally, run the app.

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
