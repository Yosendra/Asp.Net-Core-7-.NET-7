
// Dependency Problem
//  Higher-level modules depend on lower-level modules. Means, both are tightly coupled.
//
//  - The developer of higher-level module (class) SHOULD WAIT until the completion of development of lower-level module (class).
//  - Any changes made in the lower-level module effects changes in the higher-level module. Means if you make any changes
//    in the service the controller directly affected
//  - Require much code changes in to interchange an alternative lower-level module.
//  - Difficult to test a single module without effecting / testing the other module.
//
// Dependency Inversion Principle is the solution
//  - The higher-level modules (clients) SHOULD NOT depend on low-level modules (dependencies)
//    Both should depend on abstractions (interface or abstract class)
//  - Abstraction should not depend on details (both client or dependency)
//    Details (both client and dependency) should depend on abstraction
//
// Look at HomeController.cs, CitiesService.cs, ServiceContracts project, ICitiesService.cs
// ServiceContracts project in compiled file form is a '.dll' file
//
// Compile Time
//                   references                     implements
// Controller (client)  --->  Interface (abstraction)  <---  Service (dependency)
//
// Run Time
//                      calls
// Controller (client)  --->  Service (dependency)
//
// - The interface is controller by the client
// - Both client and dependency depend on abstraction

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
