
using ServiceContracts;
using Services;

// Tag Helpers for <input>, <textarea>, <select>
//   asp-for : generates "type", "name", "id", "data-validation" attribute for <input>, <textarea>, <select> tags
//
//   eg: <input asp-for="ModelProperty">
//
//       [become]
//
//       <input type="text" name="ModelProperty" id="ModelProperty" value="ModelProperty" data-val-rule="ErrorMessage" />
//   
//   Look at _ViewImports.cshtml need to add tag helper
//           Create.cshtml form at form tag
//
// Select option with asp-items and SelectListItem(), look at PersonController->Create (Get), Create (View)          

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
