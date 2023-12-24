using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace DIExample.Controllers;

public class HomeController : Controller
{
    private readonly ICitiesService _citiesService;     

    // Injection of service here in the constructor
    public HomeController(ICitiesService citiesService) // notice this
    {
        _citiesService = citiesService;                 // notice this
    }

    [Route("/")]
    public IActionResult Index()
    {
        List<string> cities = _citiesService.GetCities();

        return View(cities);
    }


}
