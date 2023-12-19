
// Layout Views
//   is a web page (.cshtml) that is responsible for containing presentation logic template (commonly
//   the html template with header, sidebar, footer, etc)
//
//  Look at _Layout.cshtml
//
//  Common naming convention to prefix the name with underscore (_)
//  generally all the layout views and partial views or even shared views are recomended
//  to be prefixed with underscore because it is easy to differentiate between the normal view
//  and shared view, maybe layout view or partial view as well
//
//  Order of Views Execution
//  _ViewImports.cshtml -> _ViewStart.cshtml -> View.cshtml -> LayoutView.cshtml
//
// Can we see the layout output directly? NO, because layout view cannot be executed directly
// but alternatively layout view can be invoked from any view in controller. So we have to create at least
// one view in a controller in order to execute the layout view

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
