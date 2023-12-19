
// Layout View for Multiple Views
// 
// Layout Views
//   - The @RenderBody() method presents only in layout view to represent the place where exactly
//     the content from the view has to be rendered
//   - The 'Layout' property of the view specifies path of the layout view
//     It can be dynamically set in the view
//   - Both View and Layout View shares the same ViewData object. So it is possible to send
//     data from view to layout, since the view execute first
//   - The css / js files imported in layout view will be applicable to view also because the content
//     of view will be merged into the layout view at run time

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
