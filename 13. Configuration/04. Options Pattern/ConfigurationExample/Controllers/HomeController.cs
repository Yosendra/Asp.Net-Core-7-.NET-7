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
        //var weatherApiSectionConfig = _configuration.GetSection("WeatherApi");
        //ViewBag.ClientId = weatherApiSectionConfig["ClientId"];
        //ViewBag.ClientSecret = weatherApiSectionConfig["ClientSecret"];

        // Bind the section configuration to our option class
        // Loads configuration values into NEW option object
        var options = _configuration.GetSection("WeatherApi").Get<WeatherApiOptions>();
        ViewBag.ClientId = options.ClientId;
        ViewBag.ClientSecret = options.ClientSecret;

        // Another way
        // Loads configuration values into EXISTING option object
        WeatherApiOptions options1 = new();
        _configuration.GetSection("WeatherApi").Bind(options1);
        ViewBag.ClientId = options1.ClientId;
        ViewBag.ClientSecret = options1.ClientSecret;

        return View();
    }
}
