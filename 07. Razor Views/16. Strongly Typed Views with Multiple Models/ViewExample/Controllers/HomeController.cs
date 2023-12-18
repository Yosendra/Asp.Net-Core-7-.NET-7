using Microsoft.AspNetCore.Mvc;
using ViewExample.Models;

// Background : We want to use multiple model in view
// Solution   : We can create a wrapper class that contain two or more model object
//              we want to use in View
//                
//              @model ModelClassName
//      View----------------------------->Model
//   @Model.PropertyName1             public Model1 Property1 { get; set; }
//   @Model.PropertyName2             public Model2 Property2 { get; set; }


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
