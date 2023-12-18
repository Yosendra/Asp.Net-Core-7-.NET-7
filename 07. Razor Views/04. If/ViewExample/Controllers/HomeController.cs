using Microsoft.AspNetCore.Mvc;

// Razor - if                       Razor - if..else
//
// @if (condition) {                @if (condition) {
//    C# / html code here              C# / html code here
// }                                }
//                                  else {
//                                     C# / html code here
//                                  }

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
