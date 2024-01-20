using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Mock DbContext
 *   Test Double
 *     A "test double" is an object that look and behave like their production equivalent objects.
 *     
 *     Runtime   : Controller -> Service (Business Logic) -> DbContext (Data Access Logic)
 *     Unit Test : Controller -> Service (Business Logic) -> Fake or Mock (Test Double)
 *     
 *     Fake : An object that provides an alternative (dummy) implementation of an interface. (full)
 *     Mock : An object on which you fix specific return value for each individual method or property
 *            without actual/full implementation of it. (partial)
 *            
 *     Install-Package Moq
 *     Install-Package EntityFrameworkCoreMock.Moq
 *     
 *     Install those packages above in ServiceTest project
 *     Use our DbContextMock in CountryServiceTest and PersonServiceTest
 *     Rename our PersonDbContext into ApplicationDbContext
 *     
 *     In ApplicationDbContext, set our DbSet as virtual in order for it to be able to be mocked
 *     
 *     Then our constructor in each service test to use our DbContextMock, then run the test
 *     
 *   Look at: CountryServiceTest, PersonServiceTest
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
