
using ServiceContracts;
using Services;

// Tag helpers for <img>
//   asp-append-version
//   ○ Generates SHA512 hash of the image file as query string parameter appended to the file path.
//   ○ It REGENERATE a new hash every time when the file is changed on the server.
//   ○ If the same file is requested multiple times, file hash will NOT be regenerated.
//
//  <img src="~/file_path" asp-append-version="true" /> [become] <img src="/file_path?v=hash_of_image_file">
//
//  Look at <img> tag at _Layout.cshtml for logo

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
