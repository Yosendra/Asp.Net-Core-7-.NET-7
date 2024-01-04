
using ServiceContracts;
using Services;

// Attribute Routing -> [Route]
//   [Route] attribute specifies route for an action method or controller.
//   The route of controller acts a prefix for route of actions.
//
//   Request to /url1               Controller
//   --------------------------> [Route("url1")]
//                                  Action 1
//
//   Request to /url2               Controller
//   --------------------------> [Route("url2")]
//                                  Action 2
//
// You can give [Route] to controller class. It acts as the prefix of action url
//
//                                   [Route("mycontroller")]    // Acts as prefix
//                                         Controller
//   Request to /mycontroller/url1
//   ---------------------------------> [Route("url1")]
//                                          Action 1
//
//   Request to /url2               
//   ---------------------------------> [Route("/url2")]
//                                          Action 2
//
//
// Look at PersonController.cs at Route Attribute
//
// Route Tokens
//   The route tokens [controller], [action] can be used to apply common-patterned routes for all action methods.
//   The route of controller acts as a prefix for the route of actions.
//
//                            [Route("[controller]/[action]")]      // Acts as prefix
//                                       Controller
//
//   Request to /mycontroller/url1      
//   -----------------------------------> Action 1

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Add services into IoC container
builder.Services.AddSingleton<ICountryService, CountryService>();   // notice here we use Singleton to make the
builder.Services.AddSingleton<IPersonService, PersonService>();     // data store alive until application shutdown

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.MapControllers();
app.Run();
