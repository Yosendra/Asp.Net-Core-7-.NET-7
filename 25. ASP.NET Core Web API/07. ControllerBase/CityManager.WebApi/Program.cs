
/* ControllerBase
* 
* Before, we put [Route("api/[controller]")] and [ApiController] attributes on each controller.
* To reduce the redundancy writing that in each controller. We create a base class to be inherited for each controller
* Then applied those two attributes at that base controller
* 
* Create new class "CustomControllerBase"
* Inherit ControllerBase on that class, then put those two attributes on CustomControllerBase
* Then delete the attirubutes at CityController
* 
* Apply it too on TestController
* 
* Run application
* 
* Look at: CityController.cs, CustomControllerBase.cs, TestController.cs
*/

using CityManager.WebApi.Entities;
using Microsoft.EntityFrameworkCore;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddControllers();
builder.Services.AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("Default")));
var app = builder.Build();

// Configure the HTTP request pipeline.
app.UseHsts();
app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();
