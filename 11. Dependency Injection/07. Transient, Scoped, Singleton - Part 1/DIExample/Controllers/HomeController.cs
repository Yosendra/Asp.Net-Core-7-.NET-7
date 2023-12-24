using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace DIExample.Controllers;

public class HomeController : Controller
{
    private readonly ICitiesService _citiesService;

    public HomeController(ICitiesService citiesService)
    {
        _citiesService = citiesService;
    }

    [Route("/")]
    public IActionResult Index()
    {
        List<string> cities = _citiesService.GetCities();

        return View(cities);
    }


}
