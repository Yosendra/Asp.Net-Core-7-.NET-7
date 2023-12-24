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
        ViewBag.MyKey = _configuration["MyKey"];
        ViewBag.MyApiKey = _configuration.GetValue<string>("MyApiKey", "the default key");

        return View();
    }
}
