using Autofac;
using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace DIExample.Controllers;

public class HomeController : Controller
{
    private readonly ICitiesService _citiesService1;
    private readonly ICitiesService _citiesService2;
    private readonly ICitiesService _citiesService3;
    //private readonly IServiceScopeFactory _serviceScopeFactory;
    private readonly ILifetimeScope _lifeTimeScope;

    public HomeController(ICitiesService citiesService1,
                          ICitiesService citiesService2,
                          ICitiesService citiesService3,
                          //IServiceScopeFactory serviceScopeFactory)   
                          ILifetimeScope serviceScopeFactory)           // now we are using Autofac
    {
        _citiesService1 = citiesService1;
        _citiesService2 = citiesService2;
        _citiesService3 = citiesService3;
        //_serviceScopeFactory = serviceScopeFactory;
        _lifeTimeScope = serviceScopeFactory;                           // now we are using Autofac
    }

    [Route("/")]
    public IActionResult Index()
    {
        List<string> cities = _citiesService1.GetCities();

        ViewBag.InstanceId_CitiesService_1 = _citiesService1.ServiceInstanceId;
        ViewBag.InstanceId_CitiesService_2 = _citiesService2.ServiceInstanceId;
        ViewBag.InstanceId_CitiesService_3 = _citiesService3.ServiceInstanceId;

        //using (IServiceScope scope = _serviceScopeFactory.CreateScope())
        using (ILifetimeScope scope = _lifeTimeScope.BeginLifetimeScope())      // now we are using Autofac
        {
            //ICitiesService citiesService = scope.ServiceProvider.GetRequiredService<ICitiesService>();
            ICitiesService citiesService = scope.Resolve<ICitiesService>();     // now we are using Autofac
            
            ViewBag.InstanceId_CitiesService_InScope = citiesService.ServiceInstanceId;
        }

        return View(cities);
    }
}
