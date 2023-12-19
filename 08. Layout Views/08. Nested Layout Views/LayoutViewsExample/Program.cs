
// Nested Layout Views
//   A layout view that has another layout view is called as 'nested layout view'
//
// Look at _Layout.cshtml and _ProductLayout.cshtml, both define its layout to "~/Views/Shared/_MasterLayout.cshtml"

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
