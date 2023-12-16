using Microsoft.AspNetCore.Mvc;
using ModelValidationExample.Models;

// [Bind]
//  Specifies that only the specified properties should be included in model binding
//  Prevent over-posting (post values into unexpected properties) especially in 'Create' scenarios.
//  It is recomended to use for security purpose
// [BindNever]
//  It is the opposite of the bind, the implementation look at 'DateOfBirth' property at Person model class

namespace ModelValidationExample.Controllers;

public class HomeController : Controller
{
    // Request from Postman with POST method to url '/register'
    // Request Body
    //   Name : scott
    //   Email : scott@example.com
    //   Phone : 123456
    //   Password : scott123
    //   Confirm Password : scott123
    //   Price : 1
    //   FromDate : 2001-06-01
    //   ToDate : 2001-06-04
    //   DateOfBirth : 2001-06-01
    //   Age : 17

    [Route("register")]     // For testing [Bind] attribute
    public IActionResult Index([Bind(nameof(Person.Name),   // Other value beside this will not be binded, 
                                     nameof(Person.Email),  // in runtime, other properties beside mentioned here will be null
                                     nameof(Person.Password),
                                     nameof(Person.ConfirmPassword))] Person person)
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

    [Route("register2")]    // For testing [NeverBind] attribute
    public IActionResult Index2(Person person)
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
