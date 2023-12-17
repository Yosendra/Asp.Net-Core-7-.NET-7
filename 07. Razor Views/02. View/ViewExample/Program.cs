
// View is a web page (.cshtml) that responsible for containing presentation logic
//  that merges data along with static design code (html)
//
// 1. Controller creates objects of ViewModel and fills data in its properties
// 2. Controller selects an approriate view and invokes the same View & supplies
//     object of ViewModel to the View
// 3. View accesses the ViewModel

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews(); // Hey Services, add all the controllers and also all the views at a time as services.
                                            // So they will be instantiated whenever you send the request.
                                            // Notice, every View also get compiled as a class internally.

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
