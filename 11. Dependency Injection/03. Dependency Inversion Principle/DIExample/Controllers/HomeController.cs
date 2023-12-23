using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace DIExample.Controllers;

public class HomeController : Controller
{
    //private readonly CitiesService _citiesService;

    // We use the service contract (interface) as the return type,
    // we don't have to wait for CitiesService finish to develop this controller
    private readonly ICitiesService _citiesService;     

    public HomeController()
    {
        // What we write here is Direct Dependency, many weakness for this practice
        // What if the CitiesService is not complete?
        // We cannot develop the controller, we have to wait CitiesService complete
        _citiesService = null;   // this one is a bad practice. We have to use 'Dependency Injection'
    }

    [Route("/")]
    public IActionResult Index()
    {
        List<string> cities = _citiesService.GetCities();

        return View(cities);
    }


}
