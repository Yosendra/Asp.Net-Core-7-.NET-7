using Microsoft.AspNetCore.Mvc;

// Razor - for
//
// @for (initialization; condition; iteration) {
// 
//   C# / html code here
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
