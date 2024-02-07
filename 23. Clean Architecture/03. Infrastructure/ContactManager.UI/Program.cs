/*
* We are migrating to the Clean Architecture by creating new solution and its project 
* grouping and structuring it into its folder like this also install packages needed through Nuget Package Manager in Infrastructure project
* 
* Infrastructure contains references to external libraries or external storage system 
* that access to the database, other email service provider, Microsoft Azure, or any other cloud service,
* or any other third-party API calls
* 
* Add project reference to the Core project from Infrastucture project
*/

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
}
app.UseStaticFiles();

app.UseRouting();

app.UseAuthorization();

app.MapRazorPages();

app.Run();
