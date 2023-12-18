using Microsoft.AspNetCore.Mvc;

// Case: if you intend to print static text in Razor Block instead of reading it as C# syntax, you can use 'Razor Literal'
//
// Razor - literal
//
// @{                           |   <text> your static text </text>
//    @: your static text       |
// }                            |


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
