using ServiceContracts;
using Services;

// Inversion of Control (IoC)
//   is a design pattern (reusable for common problem), which suggests "IoC Container"
//   for Implementation of Dependency Inversion Principle (DIP)
//
// It inverrses the control by shifting the control to IoC Container
// 'Don't call us, we will call you' pattern
// It can be implmeneted by other design pattern such as events, service locator, dependency injection, etc
//
//                              At runtime
//
//                        Give me an object that 
//                        implements the interface
//   Controller (client) --------------------------> Service (dependency)
//
//                        Supplies an object of         
//                        Service class                 IoC Container
//                     <----------------------------
//
// All dependencies should be added into the IServiceCollection (acts as IoC container)
// 
//   builder.Services.Add(
//      new Descriptor(
//          typeof(Interface),
//          typeof(Service),
//          ServiceLifeTime.LifeTime))  // Transiend, Scoped, Singleton
//
// Look at Program.cs

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Dependency Injection of our custom service class
builder.Services.Add(new ServiceDescriptor(
                            typeof(ICitiesService),
                            typeof(CitiesService),
                            ServiceLifetime.Transient));

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
