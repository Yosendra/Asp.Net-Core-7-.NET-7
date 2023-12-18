using Microsoft.AspNetCore.Mvc;
using ViewExample.Models;

// Background : with ViewData we need to typecast like this '(List<Person>?)ViewData["people"]' in the view. ViewBag make it simple for us
//
// ViewBag
//   is a property of Controller and View, that is used to access 
//   the ViewData easily
//   ViewBag is 'dynamic' type
//   The 'dynamic' type similar to 'var' keyword
//   But, it checks the data at the runtime, rather than at compilation time
//   If you try to access a non-existing property in the ViewBag, it return null
//
// Benefits fo ViewBag over ViewData
//   ViewBag syntax is easier to access its properties than ViewData
//     ViewBag.property -vs- ViewData["key"]
//   You need NOT type-cast the values while reading it
//     ViewBag.object_name.property -vs- (ViewData["key"] as ClassName).Property

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
                //Name = "Mary", DateOfBirth = DateTime.Parse("1996-8-17"), Gender = PersonGender.Female,
                Name = "Mary", DateOfBirth = null, Gender = PersonGender.Female,
            },
        };
        ViewData["appTitle"] = "Asp.NET Core Demo App";
        //ViewData["people"] = people;
        ViewBag.people = people;    // we can use ViewBag also
        // We can save in the ViewBag, then access it in ViewData or viceversa

        return View();
    }
}
