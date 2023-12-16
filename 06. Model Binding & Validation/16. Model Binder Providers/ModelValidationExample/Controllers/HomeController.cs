using Microsoft.AspNetCore.Mvc;
using ModelValidationExample.CustomModelBinders;
using ModelValidationExample.Models;

// Model Binder Providers
//  Case : you would like to use the same custom model binder for all action methods
//         wherever this type of model class is used, then you can declare it globally
//         using binder provider

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

    [Route("register")]
    public IActionResult Index(// You can hide this, since we have provide this to ModelBinderProvider
                               // [ModelBinder(BinderType = typeof(PersonModelBinder))]
                               Person person)
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
