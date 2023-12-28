using StocksApp.ServiceContract;
using StocksApp.Services;

// HttpClient
//   case: you want to read some information which is not available in your local database,
//         but outside your web application
//   
//   is a class for sending HTTP request to a specific HTTP resource (using its URL) and
//   receiving HTTP response from the same.
//   Example: making a request to third-party weather API, ChatGPT, etc
//
//        .NET App     HTTP request
//                    -------------->  
//       HTTP client                   HTTP server
//                     HTTP response 
//                    <--------------
// 
// IHttpClientFactory
//   is an interface that provides a method called '.CreateClient()' that creates a new instance
//   of HttpClient class and also automatically disposes the same instance (closes the connection)
//   immediately after usage
//
//                                                          CreateClient()
//   class DefaultHttpClientFacotry : IHttpClientFactory  -----------------> HttpClient instance 1
//                                                          CreateClient()
//                                                        -----------------> HttpClient instance 2
//   
// Look at: 'font awesome cdn' in _Layout.cshtml, FinnHubService.cs, IFinnHubService.cs,
//          HomeController, Stock.cs, appsettings.Development.json, user-secret
//
//          secret.json
//          {
//              "FinnHubToken": "cm5shnpr01qjc6l4nqr0cm5shnpr01qjc6l4nqrg"
//          }
// 
// FinnHubApiKey : cm5shnpr01qjc6l4nqr0cm5shnpr01qjc6l4nqrg (is save to store this api key in source code)
// We use user-secrets for now
//  1. Run the terminal in VS
//  2.1 Move to solution directory
//              or
//  2.2 Move to project directory
//  
//  3.1 If you choose step 2.1, then
//      Run this command "dotnet run user-secrets init --project <project_name>"
//        ex: dotnet user-secrets init --project StockApp
//              or
//  3.2 If you choose step 2.2, then
//      Run this command "dotnet user-secrets init"
//
//  4.1 if you choose step 3.1, then
//      Run this command "dotnet user-secrets set <key> <value> --project <project_name>"
//        ex: dotnet user-secrets set "FinnHubToken" "cm5shnpr01qjc6l4nqr0cm5shnpr01qjc6l4nqrg" --project StockApp
//              or
//  4.2 if you choose step 3.2, then
//      Run this command "dotnet user-secrets set <key> <value>"
//        ex: dotnet user-secrets set "FinnHubToken" "cm5shnpr01qjc6l4nqr0cm5shnpr01qjc6l4nqrg"

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Add HttpClient class service to DI container
builder.Services.AddHttpClient();

// Because we don't have the interface of 'MyService', so we inject it like this
//builder.Services.AddScoped<FinnHubService>();
builder.Services.AddScoped<IFinnHubService, FinnHubService>();

var app = builder.Build();
app.UseStaticFiles();
app.MapControllers();
app.Run();
