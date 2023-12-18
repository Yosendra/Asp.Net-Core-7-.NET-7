using Microsoft.AspNetCore.Mvc;
using ViewExample.Models;

// Strongly Typed Views
//   is a view that is bound to a specified model class.
//   It is mainly used to access the model object / model collection easily in the view
//   
//              @model ModelClassName
//      View----------------------------->Model
//   @Model.PropertyName             Data Properties
//
// Benefits of Strongly Typed Views
//  1. You will get intellisense while accessing model properties in strongly typed views,
//     since the type of model class was mentioned at @model directive
//  2. You will have only one model per one view in strongly typed views
//  3. Property names are compile-time checked, and shown as errors in case of misspelled /
//     non-existing properties in strongly typed views
//  4. Easy to identify which model is being accessed in the view

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

        return View(matchingPerson); // "Views/Home/Details.cshtml"
    }
}
