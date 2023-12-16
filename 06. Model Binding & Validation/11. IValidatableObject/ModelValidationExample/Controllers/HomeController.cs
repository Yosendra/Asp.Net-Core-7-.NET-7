using Microsoft.AspNetCore.Mvc;
using ModelValidationExample.Models;

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
