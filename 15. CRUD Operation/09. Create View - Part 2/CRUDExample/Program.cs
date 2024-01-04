
using ServiceContracts;
using Services;

// Look at PersonController and its view
//         PartialView _GridColumnHeader at Index.cshtml
//         Add Person Form action: Create() (Get Method) in PersonController, Create.cshtml
//         Create Person: Create() (Post Method), validation message at create form below submit button

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
