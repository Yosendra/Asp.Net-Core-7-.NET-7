using Autofac;
using Autofac.Extensions.DependencyInjection;
using ServiceContracts;
using Services;

// Autofac
//   is another IoC container library for .Net Core
//   Means, both are tightly-coupled.
//
//   Microsoft.Extensions.DependencyInjection [vs] Autofac
//   https://autofac.readthedocs.io/en/latest/getting-started/index.html
//
//   

var builder = WebApplication.CreateBuilder(args);

// This will change the default IoC container into Autofac IoC container
// Enable the Autofac factory
builder.Host.UseServiceProviderFactory(new AutofacServiceProviderFactory());

builder.Services.AddControllersWithViews();

//builder.Services.AddScoped<ICitiesService, CitiesService>();          // Register service to built-in IoC container
builder.Host.ConfigureContainer<ContainerBuilder>(containerBuilder =>   // Register service to Autofac IoC container
{
    containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerDependency();      // transient
    containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().SingleInstance();             // singleton
    containerBuilder.RegisterType<CitiesService>().As<ICitiesService>().InstancePerLifetimeScope();   // scoped
});

var app = builder.Build();

app.UseStaticFiles();
app.MapControllers();

app.Run();
