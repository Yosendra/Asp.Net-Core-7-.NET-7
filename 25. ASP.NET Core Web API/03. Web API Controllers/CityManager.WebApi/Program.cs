
/* Web API Controllers
* 
* Controller
*   [ApiController]
*   class ClassNameController
*   {
*     // action methods here
*   }
* 
* Should be either or both:
* • The class name should be suffixed with "Controller". Eg: ProductController
* • The [ApiController] attribute is applied to the same class or its base class
* 
* Optional:
* • Is a public class
* • Inherited from Microsoft.AspNetCore.Mvc.ControllerBase
* 
* 
* Add TestController. Right click on Cotrollers folder, then add controller.
* In category, choose API, not MVC, then choose "API Controller - Empty"
* 
* When seeing path like "api/[controller]", it means it only return data (like json or xml, indicated by "api" in url path). 
* It is convention to indicate it is a Web API url.
* 
* In launchSettings.json, in "launchUrl" key, on both "http" and "https" profile, change the value to "api/test"
* It means to make the path "api/test" is the default path we are going when first run the application.
* 
* Add the action method for TestController.
* Run the application, the look at the address bar, also put breakpoint to test it
* 
* Look at: TestController.cs, launchSettings.json
*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
