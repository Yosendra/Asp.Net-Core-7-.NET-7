using Microsoft.AspNetCore.Mvc;

// Case : by default, asp.net will sanitize any script in variable before it get printed in view.
//        If you don't want it, you can use 'Html.Raw()'
// 
// Html.Raw()
//
// @{
//     string variable = "html code"
// }
//
// @Html.Raw(variable)  // prints the html markup without encoding (converting html tags into plain text)

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
