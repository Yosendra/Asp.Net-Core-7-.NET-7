using ServiceContracts;
using Services;

// Best practice in Dependency Injection
//
// Global State in Services
//  - Avoid using static class to store some data globally for all users / all requests
//  - You may use singleton services for simple scnearios / simple amount of data.
//    In this case, prefer ConcurrentDictionary instead of Dictionary, which better
//    handles concurrent access via multiple threads.
//  - Alternatively, prefer to use Distributed Cache / Redis for any significant amount
//    of data or complex scenarios.
//
// Request State in Services
//  - Don't use scoped services to share data among services within the same request
//    because they are NOT thread-safe. Use HttpContext.Items instead.
//
// Service Locator Pattern
//  - Avoid using service locator pattern without creating a child scope
//    because it will be harder to know about dependencies of a class.
//    For example, don't invoke GetService() in the default scope that is
//    created when a new request is received. But you can use the
//    IServiceScopeFactory.ServiceProvider.GetService() within a child scope.
//  - It will be difficult to define which class define to which other service.
//    It will be difficult to track the dependency graph
//
// Caliing Dispose() method
//  - Don't invoke the Dispose() method manually for the services injected via DI.
//    The IoC container automatically invoke Dispose() at the end of its scope.
//
// Captive Dependencies
//  - Don't inject scoped or transient services in singleton service.
//    Because in this case, the transient or scoped services act as singleton
//    services inside of singleton service.
//
// Storing reference of service instance
//  - Don't hold the reference of a resolved service object.
//    It may cause memory leaks and you may have access to disposed service object

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICitiesService, CitiesService>(); 

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
