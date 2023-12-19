
// Layout Views - Sections
//   Section defines the content in the view, to be rendered in a specific place in the layout
//   Section can be rendered as optional, by using required: false parameter
//
//           View                                       Layout View
//   @section section_name
//   {                       will be rendered in
//      section content here  --------------->  @RenderSection("section_name")
//   }
//
//
//   Look at Contact.cshtml and _Layout.cshtml
//
// Case for layout section in real world project : 
//   View may import one or more script tags (javascript files) and those will be rendered on the layout view.
//   Suppose you want to render javascript script file only on certain view, then you can use this technique

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
