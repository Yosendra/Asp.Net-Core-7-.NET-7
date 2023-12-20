
// Partial Views with ViewData
//  case: is it possible to make partial view contain dynamic data?
//
//  When partial view is invoked, it receives a copy of the parent view's ViewData object.
//  So, any changes made in the ViewData in the partial view, do NOT effect the ViewData of the parent view.
//  Optionally, you can supply a custom ViewData object to the partial view, if you don't want the partial
//  view to access the entire ViewData of the parent view.
//
//     Controller                View      copy     Partial View
//  Action (ViewData) -------> ViewData ------------> ViewData
//
//  Look at HomeController, Index.cshtml, _ListPartialView.cshtml
//
//  Invoking Partial Views with ViewData
//    @{ await Html.RenderPartialAsync("patial_view_name", ViewData) }
//
//    <partial name="patial_view_name" view-data="ViewData" />


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
