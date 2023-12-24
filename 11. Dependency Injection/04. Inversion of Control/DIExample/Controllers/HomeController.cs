using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using Services;

namespace DIExample.Controllers;

public class HomeController : Controller
{
    private readonly ICitiesService _citiesService;     

    public HomeController()
    {
        _citiesService = null;
    }

    [Route("/")]
    public IActionResult Index()
    {
        List<string> cities = _citiesService.GetCities();

        return View(cities);
    }


}
