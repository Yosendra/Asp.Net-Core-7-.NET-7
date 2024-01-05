
using ServiceContracts;
using Services;

// Tag helpers for <script>
//   <script src="cdn_url" asp-fallback-src="~/local_url" asp-fallback-test="object">
//
//   [become]
//
//   <script src="cdn_url"></script>
//   <script>object || document.write("<script src="~/local_url"></script>")</script>
//
//   asp-fallback-src
//     ○ It makes a request to the specified CDN url at the "src" attribute
//     ○ It checks the value of the specified object at the "asp-fallback-test" tag helper
//     ○ if its value is null or undefined (means the script file at CDN url is not loaded), then it makes another request
//       to the local url through another script tag
//
//   Look at jquery scripts we have placed at wwwroot folder
//           create person page at the bottom, we make a section on script
//           _Layout.cshtml page, exactly on closing </body> tag, render the section optionally

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
