using ConfigurationExample;

// Inject configuration as Service
//
// in appsettings.json          in Model.cs
// {                            public class Model
//   "MasterKey": {             {
//      "Key1": "value",            public string? Key1 { get; set; }
//      "Key2": "value",            public string? Key2 { get; set; }
//      "Key3": "value",            public string? Key3 { get; set; }
//   }                          }
// }
//
// in Program.cs
// builder.Services.Configuration<Model>(builder.Configuration.GetSection("MasterKey")) 
//
// Look at: WeatherApiOptions.cs, HomeController.cs, Program.cs

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Register configuration service class as 'WeatherApiOptions' object to DI container
builder.Services.Configure<WeatherApiOptions>(
    builder.Configuration.GetSection("MasterKey"));

var app = builder.Build();
if (app.Environment.IsDevelopment() || app.Environment.IsStaging())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.MapControllers();
app.Run();
