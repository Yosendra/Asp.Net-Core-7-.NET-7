using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace DIExample.Controllers;

public class HomeController : Controller
{
    //private readonly ICitiesService _citiesService;     

    // Dependency Injection in Constructor (Constructor Injection)
    //public HomeController(ICitiesService citiesService)
    //{
    //    _citiesService = citiesService;
    //}

    [Route("/")]
    public IActionResult Index([FromServices] ICitiesService _citiesService)    // Dependency Injection in the Method
    {                                                                           // notice the [FromServices] atrribute
        List<string> cities = _citiesService.GetCities();                       // (Method Injection)

        return View(cities);
    }


}
