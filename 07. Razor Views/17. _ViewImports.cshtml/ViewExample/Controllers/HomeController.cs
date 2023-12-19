using Microsoft.AspNetCore.Mvc;
using ViewExample.Models;

// _ViewImports.cshtml
//   is a special file in the "Views" folder or its subfolder, which executes automatically before execution of a view.
//   It is mainly used to import common namespace that are imported in a view
//   You are not supposed to write any kind of content in these view imports
//
// _ViewImports.cshtml -> _ViewStart.cshtml -> View.cshtml -> LayoutView.cshtml
//
// You can also put _ViewImports.cshtml directly in Views folder. What written in this file will also be imported in all
// the views in each view which belong to other contollers. In our case, it will imported in Home View and also in Product View

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

    [Route("person-with-product")]         // Notice this action
    public IActionResult PersonWithProduct()
    {
        Person person = new()
        {
            Name = "Sara", Gender = PersonGender.Female, DateOfBirth = DateTime.Parse("2000-12-12"),
        };

        Product product = new()
        {
            Id = 1, Name = "Air Conditioner"
        };

        // Notice here we wrap both Person and Product become one model object
        PersonAndProductWrapperModel model = new()
        {
            PersonVM = person,
            ProductVM = product,
        };

        return View(model); // pass the wrapper object as a model
    }
}
