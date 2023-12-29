
// Look at: CountryServiceTest.cs, ICountryService.cs, CountryService.cs (GetAllCountries)
//          CountryResponse.cs (override .Equals() method to not compare the address reference)

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
{
    app.UseDeveloperExceptionPage();
}
app.UseStaticFiles();
app.MapControllers();
app.Run();
