using Microsoft.AspNetCore.Mvc;

// Razor - foreach
//
// @foreach (var item in collection) {
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
