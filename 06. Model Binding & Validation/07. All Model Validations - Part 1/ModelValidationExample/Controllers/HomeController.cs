namespace ModelValidationExample.Controllers;


using Microsoft.AspNetCore.Mvc;
using ModelValidationExample.Models;

public class HomeController : Controller
{
    // Request from Postman with POST method to url '/register'
    // Request Body
    //   Name : scott
    //   Email : scott@example.com
    //   Phone : 123456
    //   Password : scott123
    //   Confirm Password : abc123
    //   Price : -10 

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
