using Microsoft.AspNetCore.Mvc;

// Razor View Engine -> in order to write the server-side presentation logic in view
//  You have to use Razor Code Block and Razor Expression
//
// Razor Code Block             
//  @{                          Razor code block is a C# code block that contains one or more line of C# code
//      C# / html code here     that can contain any statements an local function
//  }
//
// Razor Expression 
//  @Expression                 Razor expression is a C# expression (accessing a field, property, or method call)
//  -or-                        that returns a value
//  @(Expression)
//
// Quick Facts about View
//  1. View contains HTML markup with Razor markup (C# code in view to render dynamic content)
//  2. Razor is the view engine that defines syntax to write C# code in the view
//     @ is the syntax of Razor syntax
//  3. View is NOT supposed to have lots of C# code
//     Any code written in the view should relate to presenting the content (presentation logic)
//  4. View should neither directly call the business model, nor call the controller's action method
//     But it can send requests to the controller
//  5. View directly access the ViewModel, not the BusinessModel (service oject to perform business logic and operation)

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
