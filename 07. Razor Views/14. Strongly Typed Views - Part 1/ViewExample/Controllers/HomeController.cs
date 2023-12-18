using Microsoft.AspNetCore.Mvc;
using ViewExample.Models;

// Strongly Typed Views
//   is a view that is bound to a specified model class.
//   It is mainly used to access the model object / model collection easily in the view
//   
//              @model ModelClassName
//      View----------------------------->Model
//   @Model.PropertyName             Data Properties


namespace ViewExample.Controllers;

public class HomeController : Controller
{
    [Route("home")]
    [Route("/")]
    public IActionResult Index()
    {
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

        return View(people);    // Controller supply the model for the View
    }
}
