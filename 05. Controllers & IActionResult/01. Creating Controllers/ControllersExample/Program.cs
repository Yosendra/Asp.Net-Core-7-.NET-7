
// Controller is a class that is used to group-up a set of actions (or action methods)
//  each action method act as an endpoint, which can be requested by spesific url
// Action methods do perform certain operation when a request is received & returns the result (response)

// Look at 'HomeController.cs' file

using ControllersExample.Controllers;

var builder = WebApplication.CreateBuilder(args);

//builder.Services.AddTransient<HomeController>();    // register HomeController in dependency injection, but what if there are hundred controller?

// Use this to automatically detect all files with suffix '-Controller.cs'
//  to register them in dependency injection as controller
builder.Services.AddControllers();

var app = builder.Build();

// After registering controller we can map it in routing
//app.UseRouting();
//app.UseEndpoints(endpoints =>
//{   
//    // Rather than register action method to a path one-by-one like previous using '.Map()', we can use statement
//    //  below only to register all controller automatically, for mapping to a path, we use Attribut in controller class for now
//    endpoints.MapControllers();
//});
app.MapControllers();   // or you can just write this single statement without call .UseRouting() and .UseEndpoints() to enable controller like before

app.Run();
