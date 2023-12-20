
// Partial Views
//   is a razor markup file (.cshtml) that can't be invoked individually from the controller,
//   but can be invoked from any view within the same web application.
//
//               View                    will invoke         Partial View
//   <partial name="patial_view_name"> ---------------> patial view content here
//
// Look at _ListPartialView.cshtml, Index.cshtml, ~/Views/_ViewImports.cshtml, About.cshtml
//
// Invoking Partial Views
//
//   <partial name="partial_view_name" />
//   Returns the content to the parent view
//
//   @await Html.PartialAsync("partial_view_name")
//   Returns the content to the parent view
//
//   @{ await Html.RenderPartialAsync("partial_view_name") }
//   Streams the content to the browser

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
