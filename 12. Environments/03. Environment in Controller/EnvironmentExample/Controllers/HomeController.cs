using Microsoft.AspNetCore.Mvc;

namespace EnvironmentExample.Controllers;

public class HomeController : Controller
{
    private readonly IWebHostEnvironment _webHostEnvironment;

    // Inject environment object so it become accessible in this controller
    public HomeController(IWebHostEnvironment webHostEnvironment)
    {
        _webHostEnvironment = webHostEnvironment;
    }

    [Route("/")]
    public IActionResult Index()
    {
        ViewBag.CurrentEnvironment = _webHostEnvironment.EnvironmentName; // notice here
        return View();
    }

    [Route("some-route")]
    public IActionResult Other()
    {
        return View();
    }
}
