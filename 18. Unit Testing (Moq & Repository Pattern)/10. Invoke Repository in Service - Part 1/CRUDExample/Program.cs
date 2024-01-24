using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Invoke Repository in Service
 *   Inject ICountryRepository in CountryService constructor
 *   Change many implementation that involve data access logic through db, now, into repository
 *   
 * Unit Testing
 *   No Repository
 *     Controller -> Service -> DbContext
 *     Unit Test  -> Service -> DbContext Mock
 *   
 *   With Repository
 *     Controller -> Service -> Repository -> DbContext
 *     Controller -> Service -> Repository Mock
 *   
 *   There is some unintended code in CountryServiceTest due to our changes to use Repository pattern.
 *   We need to mock the the repository, then inject the mock object to the Service constructor
 *   We will fix it later.
 *   
 *   Look at: CountryService.cs, CountryServiceTest.cs
 */

var builder = WebApplication.CreateBuilder(args);
builder.Services
    .AddDbContext<ApplicationDbContext>(options => options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")))
    .AddServices()
    .AddControllersWithViews();

var app = builder.Build();
if (app.Environment.IsDevelopment())
    app.UseDeveloperExceptionPage();

RotativaConfiguration.Setup("wwwroot");
app.UseStaticFiles();
app.MapControllers();
app.Run();
