using ServiceContracts;
using Services;

// Dependency Injection
//   Dependency injection (DI) is a design pattern, which is a technique for achieving
//   "Inversion of Control (IoC)" between clients and their dependencies.
//
// It allows you to inject (supply) a concrete implementation object of a low-level
// component into a high-level component.
// The client class receives the dependency object as a parameter either
// in the constructor or in a method.
//
//                              At runtime
//
//                        Give me an object that 
//                        implements the interface
//   Controller (client) -------------------------->  Service : IService
//                                                      (dependency)
//   receive the
//   the dependency       Supplies an object of
//   object as a          Service class                 
//   parameter          <---------------------------- IoC / DI Container
//                                 
//                                       
//                     

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

// Register the service class & Its interface to the container
builder.Services.Add(new ServiceDescriptor(
                            typeof(ICitiesService),
                            typeof(CitiesService),
                            ServiceLifetime.Transient));

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
