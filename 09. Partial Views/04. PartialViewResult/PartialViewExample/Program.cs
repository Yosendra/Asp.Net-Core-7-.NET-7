
// PartialViewResult
//   can represent the content of a partial.
//   Generally useful to fetch partial view's content into the browser by making an
//   asynchronous request (XMLHttpRequest / fetch request) from the browser
//
//   asynchronous request ----> Controller (action return 'PartialViewResult') ----> Response body: (content of partial view)
//
// Case: it like ajax concept, not to fully reload the whole page. Just a part of the page
// Look at HomeController, _ListPartialView.cshtml, Index.cshtml

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
