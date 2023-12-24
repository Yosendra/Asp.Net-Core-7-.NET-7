
// Options Pattern
//   is all about specifying the exact properties that you want to read from the configuration
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
// Option pattern uses custom classes to specify what configuration settings are
// to be loaded into properties.
// Example: reading the specific connection string out of many configuration setting
//
// The option class should be a non-abstract class with a public parameterless constructor
// Public read-write properties are bound. Fields are not bound.

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
