
// Hierarchical Configuration
//   What if inside key is a an another object?
//
//   in appsettings.json
//   {
//      "MasterKey": {
//          "Key1": "Value1",
//          "Key2": "Value2",
//      }
//   }
//
//   to read configuration -> Configuration["MasterKey:Key1"]
//   
//
// Look at: HomeController.cs, index view

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
