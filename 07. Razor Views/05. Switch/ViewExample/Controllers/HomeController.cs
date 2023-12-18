using Microsoft.AspNetCore.Mvc;

// Razor - Switch
//
// @switch (variable) {
//
//   case value1: C# / html code here; break;
//
//   case value2: C# / html code here; break;
//
//   default: C# / html code here; break;
//
// }

namespace ViewExample.Controllers;

public class HomeController : Controller
{
    [Route("home")]
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }
}
