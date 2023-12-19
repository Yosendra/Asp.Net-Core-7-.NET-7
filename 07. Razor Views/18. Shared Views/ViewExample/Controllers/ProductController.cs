using Microsoft.AspNetCore.Mvc;

namespace ViewExample.Controllers;

public class ProductController : Controller
{
    [Route("products/all")]
    public IActionResult All()
    {
        // Do not find the view file in "View\Product\All.cshtml"
        // Find it in "View\Shared\All.cshtml"
        return View();
    }
}
