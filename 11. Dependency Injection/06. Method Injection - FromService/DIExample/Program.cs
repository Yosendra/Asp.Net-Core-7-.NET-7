using ServiceContracts;
using Services;

// Method Injection - [FromServices]
//  We can do dependcy injection in a method parameter.
//  Look at HomeController.cs

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Add(new ServiceDescriptor(
                            typeof(ICitiesService),
                            typeof(CitiesService),
                            ServiceLifetime.Transient));

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
