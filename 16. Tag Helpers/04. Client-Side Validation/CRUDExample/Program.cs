
using ServiceContracts;
using Services;

// Client Side Validation
//
// Data annotations on model properties                     Import jQuery Validation Scripts
// [Required]                                                - jquery.min.js
// public Datatype PropertyName {get; set;}                  - jquery.validate.min.js
//                                                           - jquery.validate.unobtrusive.min.js
// "data-*" attributes in html tags                         (The order must be fixed when imported)
// [auto-generated] with "asp-for" helper
//
// Look at Create.cshtml, at the bottom we import the jquery script
//         using of asp-validation-for at <span> tag
//         asp-validation-summary at below the submit button


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
