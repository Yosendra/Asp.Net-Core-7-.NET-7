using Microsoft.AspNetCore.Mvc;
using ViewComponentExample.Models;

namespace ViewComponentExample.Controllers;

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

    [Route("friend-list")]
    public IActionResult LoadFriendList()
    {
        PersonGridModel personGridModel = new()
        {
            Title = "Persons",
            Persons =
            {
                new Person() { Name = "John", JobTitle = "Manager" },
                new Person() { Name = "Jones", JobTitle = "Assistant Manager" },
                new Person() { Name = "William", JobTitle = "Clerk" },
            }
        };

        // Notice the return we write, this is for ajax request
        // The rendered content of the view component will be returned
        // as response body back to the browser
        return ViewComponent("Grid", new { model = personGridModel });
    }
}
