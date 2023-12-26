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
// Look at: 'font awesome cdn' in _Layout.cshtml, MyService.cs, 

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllersWithViews();

// Add HttpClient class service to DI container
builder.Services.AddHttpClient();

var app = builder.Build();
app.UseStaticFiles();
app.MapControllers();
app.Run();
