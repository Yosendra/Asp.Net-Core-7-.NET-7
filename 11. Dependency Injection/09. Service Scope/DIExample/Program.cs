using ServiceContracts;
using Services;

// Service Scope
//                                                    IServiceScopeFactory.CreateScope()  
//                 | Request 1 ---> Request Scope 1 ----------------------------------------> Child Scope 1  
//                 | 
//    Root Scope   |
//                 |
//                 | Request 2 ---> Request Scope 2
//
// The scope or the lifetime of the service object is per-scope defined in 'using' statement,
// not per-request like Transient & Scoped.
// The service object will be automatically disposed at the end of 'using' statement,
// not at the end of request like Transient & Scoped.
//
// In case of Entity Framework, the service instance lifetime of the database services will
// be automatically managed by Entity Framework itself.
//
// The approach of Child Scope can be practices only when you are implementing
// non Enitity Framework like if you are writing your own ADO.NET code
// for database connection
//
// Look at HomeController

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Add(new ServiceDescriptor(
                            typeof(ICitiesService),
                            typeof(CitiesService),
                            ServiceLifetime.Scoped));

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
