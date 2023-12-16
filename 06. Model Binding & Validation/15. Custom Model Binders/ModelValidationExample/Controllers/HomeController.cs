using Microsoft.AspNetCore.Mvc;
using ModelValidationExample.CustomModelBinders;
using ModelValidationExample.Models;

// Custom Model Binder
//  Case : in the request body there are FirstName and LastName values sent,
//         we want to bind them into single property 'Name'

namespace ModelValidationExample.Controllers;

public class HomeController : Controller
{
    // Request from Postman with POST method to url '/register'
    // Request Body (form field)
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

    // Notice the validation with attribute in model is still working as it is

    [Route("register")]
    public IActionResult Index([FromBody] // Use the our custom ModelBinder. our responsibility
                                          // is reading the value provider, then create and return the object model
                               [ModelBinder(BinderType = typeof(PersonModelBinder))] Person person)
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
