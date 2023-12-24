
// Process Level Environment
//  Remember that this launchsettings.json file is only for developer machine
//  in case of staging or production server, you will not deploy this file.
//  Then beside Development machine how do we set the environment value?
//
//         _________Terminal_________
//        |      .Net Core Host     |
//        |-------------------------|
//        |  Environment Variables  |
//        |    Application Code     |
//        ---------------------------
//  The environment variables are stored & accessible
//  within the same process only
//        
//  We have to deploy our application to specific folder then run the app using
//  terminal. Through this terminal we set the environment
//
//  Go to the project folder, then run this script in the terminal
//   'dotnet run'
//      this still read launchSettings.json file because the file is there
//   'dotnet run --no-launch-profile'
//      to ignore the launchSettings.json file, will automatically set the
//      environment to production
//      
// Enter $Env.ASPNETCORE_ENVIRONMENT="Development" before enter command above to custom the environment in terminal
//  $Env.ASPNETCORE_ENVIRONMENT="Development" --> Power Shell
//  set ASPNETCORE_ENVIRONMENT="Development" --> Command Prompt

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();
var app = builder.Build();
 
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
}

app.UseStaticFiles();
app.MapControllers();

app.Run();
