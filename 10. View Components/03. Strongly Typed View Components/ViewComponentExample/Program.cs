
// Strongly Typed View Components
//   Strongly Typed View Components's view tightly bound to a specified model class.
//   So, it gets all the benefits of strongly typed view.
//
//
//    ViewComponent view   @model ModelClassName     Model
//   Presentation Logic &  --------------------> Data Properties
//   @Model.PropertyName
//
//   Look at GridViewComponent and its view

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
