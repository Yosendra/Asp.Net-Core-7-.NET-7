using Microsoft.AspNetCore.Mvc;

namespace LayoutViewsExample.Controllers;

public class ProductController : Controller
{
    [Route("products")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("search-products/{id?}")]
    public IActionResult Search(int? id)
    {
        ViewBag.ProductId = id;
        return View();
    }

    [Route("order-products")]
    public IActionResult Order()
    {
        return View();
    }
}
