using Microsoft.AspNetCore.Mvc;
using Services;

namespace DIExample.Controllers;

public class HomeController : Controller
{
    // These are the services
    private readonly CitiesService _citiesService;

    public HomeController()
    {
        // create object of CitiesService class
        _citiesService = new CitiesService();   // this one is a bad practice. We have to use 'Dependency Injection'
    }

    [Route("/")]
    public IActionResult Index()
    {
        // The logic how to get the data is in the Service class
        // Controller just invoke the service and then get the data
        List<string> cities = _citiesService.GetCities();

        return View(cities);
    }


}
