
using ServiceContracts;
using Services;

// Tag Helpers for <form>
//   asp-controller & asp-action : generates route url for the specified action method with "controller/action" route pattern
//   eg. <form asp-controller="ControllerName" asp-action="ActionName">
//       </form>
//
//       [become]
//
//       <form action="~/ControllerName/ActionName">
//       </form>
//       
//
//   Look at _ViewImports.cshtml need to add tag helper
//           Create.cshtml form at form tag

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
