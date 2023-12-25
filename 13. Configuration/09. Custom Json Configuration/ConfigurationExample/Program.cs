using ConfigurationExample;

// Custom Json Configuration
//
// in custom-file.json          in Model.cs
// {                            public class Model
//   "MasterKey": {             {
//      "Key1": "value", ---------> public string? Key1 { get; set; }
//      "Key2": "value", ---------> public string? Key2 { get; set; }
//      "Key3": "value", ---------> public string? Key3 { get; set; }
//   }                          }
// }
//
// Create your own config json file, ex: MyOwnConfig.json. Define your key-value pair
// Now add our own config json file as configuration source in Program.cs
// 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

builder.Services.Configure<WeatherApiOptions>(
    builder.Configuration.GetSection("WeatherApi"));

// Load MyOwnConfig.json
builder.Host.ConfigureAppConfiguration((hostContext, config) =>
{
    // optional true, in case the file doesn't exist, there will be no exception
    // reloadOnChange true, in case there is some changes in MyOwnConfig.json, the application will automatically restart
    config.AddJsonFile("MyOwnConfig.json", optional: true, reloadOnChange: true);
});

var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.MapControllers();
app.Run();
