using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers;

public class PersonController : Controller
{
    [Route("person/index")]
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }
}
