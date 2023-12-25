using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.Controllers;

public class HomeController : Controller
{
    private readonly WeatherApiOptions _weatherApiOptions;

    public HomeController(IOptions<WeatherApiOptions> weatherApiOptions)
    {
        _weatherApiOptions = weatherApiOptions.Value;
    }

    [Route("/")]
    public IActionResult Index()
    {
        ViewBag.ClientId = _weatherApiOptions.ClientId;
        ViewBag.ClientSecret = _weatherApiOptions.ClientSecret;

        return View();
    }
}
