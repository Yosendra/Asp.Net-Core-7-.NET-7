using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;

namespace ConfigurationExample.Controllers;

public class HomeController : Controller
{
    //private readonly IConfiguration _configuration;
    private readonly WeatherApiOptions _weatherApiOptions;            // notice this

    //public HomeController(IConfiguration configuration)
    public HomeController(IOptions<WeatherApiOptions> weatherApiOptions)        // using the injected WeatherApiOptions
    {
        //_configuration = configuration;
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
