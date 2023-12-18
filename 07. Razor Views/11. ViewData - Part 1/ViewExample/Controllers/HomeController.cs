using Microsoft.AspNetCore.Mvc;
using ViewExample.Models;

// Background : View should be simple. Not defining data. Data is provided by Controller.
//
// ViewData
//   is a dictionary object that is automatically created up on receiving a
//   request and will be automatically deleted before sending response to the client.
//   It is mainly used to send data from controller to view.

namespace ViewExample.Controllers;

public class HomeController : Controller
{
    [Route("home")]
    [Route("/")]
    public IActionResult Index()
    {
        // ViewData is inherited from Controller class
        // Here we moved appTitle and people list in the view to the controller
        
        List<Person> people = new()
        {
            new Person()
            {
                Name = "John", DateOfBirth = DateTime.Parse("2000-12-12"), Gender = PersonGender.Male,
            },
            new Person()
            {
                Name = "Mary", DateOfBirth = DateTime.Parse("1996-8-17"), Gender = PersonGender.Female,
            },
        };
        ViewData["appTitle"] = "Asp.NET Core Demo App";
        ViewData["people"] = people;

        return View();
    }
}
