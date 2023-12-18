using Microsoft.AspNetCore.Mvc;

// Razor - Local Function
//
// @{
//      return_type function_name(parameters) {
//      
//          C# / html code here
//          
//      } 
// }
//
// Local function are callable only within the same view


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
