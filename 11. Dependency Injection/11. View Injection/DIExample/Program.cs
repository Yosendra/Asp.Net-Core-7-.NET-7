using ServiceContracts;
using Services;

// View Injection
//  in APS.NET the view are internally compiled as their own classes. So like you can inject services object
//  in Controller class and other service classes, you can also it service to View as well
//
//                                               Supplies an object of Service Class
//  @inject IService serviceReferenceVariable  <-------------------------------------- Service : IService 
//  @{                                                                                   (Dependency)
//      serviceReferenceVariable.ServiceMethod()                    
//  }                                                                                  IoC / DI Container
//
//  Look at _ViewImports.cshtml, Index.cshtml

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.AddScoped<ICitiesService, CitiesService>(); 

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
