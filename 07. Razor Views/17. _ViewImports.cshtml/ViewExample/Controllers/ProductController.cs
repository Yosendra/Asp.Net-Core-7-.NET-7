using Microsoft.AspNetCore.Mvc;

namespace ViewExample.Controllers;

public class ProductController : Controller
{
    [Route("products/all")]
    public IActionResult All()
    {
        return View();
    }
}
