using Microsoft.AspNetCore.Mvc;
using PartialViewExample.Models;

namespace PartialViewExample.Controllers;

public class HomeController : Controller
{
    [Route("/")]
    public IActionResult Index()
    {
        return View();
    }

    [Route("about")]
    public IActionResult About()
    {
        return View();
    }

    [Route("Programming-Language")]
    public IActionResult ProgrammingLanguage()
    {
        ListModel listModel = new ListModel()
        {
            Title = "Programming Language List",
            ListItems = { "C#", "PHP", "Javascript", "Python", }
        };

        // Return partial view content
        return PartialView("_ListPartialView", listModel);
    }
}
