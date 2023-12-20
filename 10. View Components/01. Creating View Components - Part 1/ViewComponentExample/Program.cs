
// View Components
//   is a combination of a class (derived from Microsoft.AspNetCore.ViewComponent)
//   that supplied data, and a partial view to render that data.
//
//                       View                            invoke     View Component
//   @await Component.InvokeAsync("view_component_name") ------> View Component Class
//                                                                  return View()
//
//                              return the rendered html            Partial View
//                         <-------------------------------- partial view content here
//
// Look at Index.cshtml, GridViewComponent.cs, About.cshtml
//
// Invoking View Component
//   1. @await Component.InvokeAsync("View Component Name")
//   2. <vc:view-component-name>
//
// View component renders a chunk rather than a whole response.
// Include the same seperation-of-concerns and testablity benefits found with a controller and view.
// Should be either suffixed with the word "ViewComponent" or should have [ViewComponent] attribute.
// Optionally, it can inherit from Microsoft.AspNetCore.Mvc.ViewComponent;
//
// Advice: When to View Component over the Partial View?
//         When we have some logic to execute before actual view invokes
//         such as retrieving the data from databases or invoking a method to fetch the data
//         or doing calculation or any other programming logic should be executed
//         before the actual view, then it is a good time to use View Component
//         along with that you have the same benefit as the partial view which is 'Reusability'

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
