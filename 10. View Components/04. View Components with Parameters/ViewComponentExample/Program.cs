
// View Components with Parameters
//   You can supply one or more parameters to the new component class.
//   The parameters are received by InvokeAsync method of the component class.
//   All the parameters of view component are mandatory (must supply a value)
//
//                  Parent View
//   @await Component.InvokeAsync("view_component_name",
//                                 new {
//                                      arg1 = value1,
//                                      arg2 = value2,
//                                 })
//
//              ViewComponent class
//  InvokeAsync(datatype parame1, datatype param2) {}
//
// Invoking ViewComponent with parameters
//   1. @await Component.InvokeAsync("view_component_name",
//                                    new {
//                                      arg1 = value1,
//                                      arg2 = value2,
//                                    })
//                                    
//   2. <vc:view-component-name param="value" />
//
//  Look at GridViewComponent.cs and its partial view, About & Index view

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
