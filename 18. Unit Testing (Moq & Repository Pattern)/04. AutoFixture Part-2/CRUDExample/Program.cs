using CRUDExample.Bootstrap;
using Entities;
using Microsoft.EntityFrameworkCore;
using Rotativa.AspNetCore;

/* Auto Fixture
 *   generates objects of the specified classes and their properties with some fake values based their data type.
 *     
 *     AutoFixture               AutoFixture
 *       Create object    --->     Fill dummy values into properties
 *       
 *     Normal object creation    With AutoFixture
 *       new ModelClass() {      Fixture.Create<ModelClass>();
 *         Porperty1 = value,    // Initializes all properties of the specified model class with dummy values
 *         Property2 = value,
 *       }
 *       
 *   Install AutoFixture package at ServiceTest project 
 *   Use the AutoFixture at the service test file
 *   We add private field for fixture and initialize it in the contructor
 *   
 *   Look at: PersonServiceTest at AddPerson_Success_ReturnPersonResponseWithId()
 *   
 *   Now we implement AutoFixture for all test case method in PersonServiceTest
 *   
 *   CountryServiceTest still not implemented
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
