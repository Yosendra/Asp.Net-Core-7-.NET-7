using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Fluent Assertion
 *   Fluent Assertion are a set of extension method to make the assertions in unit testing readable and human-friendly
 *   
 *   Install-Package FluentAssertions
 *   
 *   Assert                                 Fluent Assertion
 *     Assert.Equal(expected, actual)         actual.Should().Be(expected)
 *   
 *   Look at: PersonServiceTest.cs
 *   
 *   Complete for PersonServiceTest. CountryServiceTest not yet...
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
