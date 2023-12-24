using ServiceContracts;
using Services;

// Service Lifetime in DI (Transient, Scoped, Singleton)
//   A service lifetime indicates when a new object of the service has to be
//   created by the IoC / DI container
//
// Transient -> Per-injection
// Scoped -> Per-scope (browser request)
// SIngleton -> For entire application lifetime
//
// Transient, we injected the same transient service into three different controllers or
//   maybe three different other services. So everytime when transient service is injected
//   a new object will be created for that service class. For example CitiesService injected
//   3 times, so internally 3 objects of the CitiesService gets created.
//   That's way it is called per injection.
//
// Scoped, for every scope a new service object will be created but the scope lifetime in
//   general is a browser reqeust. Same service object is used if the same service object is
//   injected in another place of application.
//
// Singleton, service object created when it is injected into any class for the first time.
//   But later the same service object will be reused every time. So new object of singleton
//   service will not be created until the application shutdown
//
// ○ Transient
//   Transient lifetime service object are created each time when they are injected.
//   Service instances are disposed at the end of the scope (usually, a browser request).
// ○ Scoped
//   Scoped lifetime service objects are created once per a scope (usually, a browser request).
//   Service instances are disposed at the end of the scope (usually, a browser request).
// ○ Singleton
//   Singleton lifetime service objects are created for the first time when they are requested.
//   Service instances are disposed at application shutdown.
//
// The proof of services lifetime
//  Look at CitiesService.cs, ICitiesService.cs, HomeController.cs,
//  Program.cs at dependency lifetime change it to another value (Transient, Scoped, Singleton)
//  Index view
//
// When to use which one?
//  - When you want to store some data temporarily such as cache services that is common data for
//    for all the users and all the reqeust. Choose SINGLETON. eg: in-memory collection as alternatives to database.
//  - But database services such as Entity Framework db context, so those will be used as SCOPED services because
//    when a new browser request is received by the application a new database connection should be opened
//    and that database connection should be closed at the end of the request. So for each request, a new database connection.
//  - When you want your service to be short-lived that means only for one time for one controller
//    then TRANSIENT services are best. For example there is a service that encrypts the data, suppose you don't want
//    to share the encrypted data for other requests or even for other service instances. So you want to create a new
//    service object for each invocation means each injection. Another example there is service that sends an email
//    automatically so that service should be instantiated for each injection, for such services you will use transient.


var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Add(new ServiceDescriptor(
                            typeof(ICitiesService),
                            typeof(CitiesService),
// Interchange the service lifetime
//ServiceLifetime.Transient));
//ServiceLifetime.Scoped));
ServiceLifetime.Singleton));

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
