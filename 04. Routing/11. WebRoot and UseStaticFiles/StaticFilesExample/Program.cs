
// WebRoot and UseStaticFiles

// The default WebRoot folder is "wwwroot". Can be configured manually.
// All static default will be placed inside wwwroot folder

using Microsoft.Extensions.FileProviders;

var builder = WebApplication.CreateBuilder(new WebApplicationOptions()
{
    WebRootPath = "MyRoot", // customize the 'wwwroot' folder name to 'MyRoot'. Folder 'wwwroot' must be there, if not there will be an exception
});
var app = builder.Build();

// This work for "Myroot", as we configure above
app.UseStaticFiles();   // to enable static files, test by accessing path '/sample.txt'

// This work for "MyWebRoot", we need some tweak here
app.UseStaticFiles(new StaticFileOptions()
{
    FileProvider = new PhysicalFileProvider(builder.Environment.ContentRootPath + @"\MyWebRoot"),
});

app.MapGet("/", () => "Hello World!");

app.Run();
