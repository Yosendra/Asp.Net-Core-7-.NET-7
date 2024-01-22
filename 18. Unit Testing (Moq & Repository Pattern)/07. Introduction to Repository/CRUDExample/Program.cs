using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Repository
 *   or Repository Pattern is an abstraction between Data Access Layer (EF DbContext) and Business Logic Layer (Service) of the application
 *   
 *   No Repository
 *     Controller -> Business Logic (Service) -> Data Access Logic (DbContext)
 *     
 *   With Repository
 *     Controller -> Business Logic (Service) -> Repository -> Data Access Logic (DbContext)
 *   
 *   Look at: RepositoryContracts project, ICountryRepository.cs, IPersonRepository.cs
 *   
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
