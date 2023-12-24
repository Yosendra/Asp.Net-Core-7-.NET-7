using Microsoft.AspNetCore.Mvc;

namespace EnvironmentExample.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    [Route("some-route")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("some-route")]   // notice this, same route as index, thrown exception in runtime
    public IActionResult Other()
    {
        return View();
    }
}
