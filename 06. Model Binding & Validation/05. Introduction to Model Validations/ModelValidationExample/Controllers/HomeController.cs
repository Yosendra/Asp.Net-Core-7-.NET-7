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
    // Instead we put the validation on controller
    //  we can put the validation in model class
    public IActionResult Index(Person person)
    {
        return Content($"{person}");
    }
}
