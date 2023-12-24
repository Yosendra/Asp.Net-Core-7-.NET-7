using ServiceContracts;
using Services;

// Service Lifetime in DI - AddTransient(), AddScoped(), AddSingleton()
//
// There are more shorthand methods to register services into the service collection
//  ○ Transient
//      builder.Services.AddTransient<IService, Service>();
//  ○ Scoped
//      builder.Services.AddScoped<IService, Service>();
//  ○ Singleton
//      builder.Services.AddSingleton<IService, Service>();

var builder = WebApplication.CreateBuilder(args);

builder.Services.AddControllersWithViews();

builder.Services.Add(new ServiceDescriptor(
                            typeof(ICitiesService),
                            typeof(CitiesService),
                            ServiceLifetime.Scoped));

// More concise than above
builder.Services.AddSingleton<ICitiesService, CitiesService>();
builder.Services.AddTransient<ICitiesService, CitiesService>();
builder.Services.AddScoped<ICitiesService, CitiesService>(); 

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
