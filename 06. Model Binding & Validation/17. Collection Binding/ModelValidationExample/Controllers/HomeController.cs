using Microsoft.AspNetCore.Mvc;
using ModelValidationExample.Models;

// Collection Binding
//  Case : If you want to bind value in request body with multiple value.

namespace ModelValidationExample.Controllers;

public class HomeController : Controller
{
    // Request from Postman with POST method to url '/register'
    // Request Body (form field)
    //   Name : Alex
    //   FirstName : Scott
    //   LastName : Pilgrim
    //   Email : scott@example.com
    //   Phone : 123456
    //   Password : scott123
    //   ConfirmPassword : scott123
    //   Price : 1
    //   DateOfBirth : 2001-06-01
    //   FromDate : 2001-06-01
    //   ToDate : 2001-06-04

    // Default binder understand this if you want to bind collection/array. Notice the index
    //   Tags[0] : #dotnet
    //   Tags[1] : #python

    [Route("register")]
    public IActionResult Index(Person person)
    {
        if (!ModelState.IsValid)
        {
            List<string> errorList = new();

            errorList = ModelState.Values
                .SelectMany(value => value.Errors)
                .Select(error => error.ErrorMessage).ToList();

            string errorMessage = string.Join('\n', errorList);
            return BadRequest(errorMessage);
        }

        return Content($"{person}");
    }
}
