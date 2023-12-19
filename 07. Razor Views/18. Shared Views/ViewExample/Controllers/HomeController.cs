using Microsoft.AspNetCore.Mvc;
using ViewExample.Models;

// Shared Views
//   are placed in "Shared" folder in "Views" folder
//   They are accessible from any contoller, if the view is NOT present in the "Views\ControllerName" folder

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

        return View(people);
    }

    [Route("person-details/{name}")]
    public IActionResult Details(string? name)
    {
        if (name == null)
            return Content("Person name can't be null");

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

        Person? matchingPerson = people.Where(p => p.Name == name).FirstOrDefault();

        return View(matchingPerson);
    }

    [Route("person-with-product")]
    public IActionResult PersonWithProduct()
    {
        Person person = new() { Name = "Sara", Gender = PersonGender.Female, DateOfBirth = DateTime.Parse("2000-12-12") };
        Product product = new() { Id = 1, Name = "Air Conditioner" };

        PersonAndProductWrapperModel model = new()
        {
            PersonVM = person,
            ProductVM = product,
        };

        return View(model);
    }

    [Route("home/all-products")]
    public IActionResult All()
    {
        // Do not find the view file in "View\Home\All.cshtml"
        // Find it in "View\Shared\All.cshtml"
        return View();
    }
}
