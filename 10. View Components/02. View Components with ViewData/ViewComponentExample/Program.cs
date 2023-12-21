
// View Components with ViewData
//   The ViewComponent class can share ViewData object to the ViewComponent view
//
//   View Component Class           View Component View
//      InvokeAsync()                Presentation Logic
//        ViewData    -------------->     ViewData
//
// Look at PersonGridModel.cs, GridViewComponent.cs, GridViewComponent view, Index & About view invoking the component
//
var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
