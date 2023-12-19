
// _ViewStart.cshtml gets executed before execution of the view and it used to specify the
//  common layout view for multiple views in the same folder
//
// The local _ViewStart.cshtml can override the global _ViewStart.cshtml
//
// Notice in every view we connect to layout by this statement 'Layout = "~/Views/Shared/_Layout.cshtml";'
// We will move this statement to _ViewStart.cshtml the global one, so we just write it once only
//
// Don't confuse _ViewStart.cshtml is not the layout, it is just a special file to set the layout view path
// Take a look at _ViewStart.cshtml, _ProductLayout.cshtml, the view files, and Search.cshtml where we override the Layout path

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
