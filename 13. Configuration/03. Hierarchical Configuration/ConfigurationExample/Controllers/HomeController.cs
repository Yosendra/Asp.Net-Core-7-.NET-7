using Microsoft.AspNetCore.Mvc;

namespace ConfigurationExample.Controllers;

public class HomeController : Controller
{
    private readonly IConfiguration _configuration;

    public HomeController(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    [Route("/")]
    public IActionResult Index()
    {
        // reading multi-level configuration key
        //ViewBag.ClientId = _configuration["WeatherApi:ClientId"];
        //ViewBag.ClientSecret = _configuration.GetValue<string>("WeatherApi:ClientSecret", "the default client secret");

        var weatherApiSectionConfig = _configuration.GetSection("WeatherApi");
        ViewBag.ClientId = weatherApiSectionConfig["ClientId"];
        ViewBag.ClientSecret = weatherApiSectionConfig["ClientSecret"];

        return View();
    }
}
