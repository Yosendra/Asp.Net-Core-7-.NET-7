namespace ModelValidationExample.Controllers;

// Model State is a property of controller base that is available in all the action
//  methods of the controller used to check the status of the validation of model object

// Property of ModelState object
//  IsValid -> Specifies whether there is at least one validation error or not. (true or false)
//  Values  -> Contains each model property value with corresponding "Errors" property that
//             contains list of validation errors of that model property.
//  ErrorCount -> Returns number of errors.

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
        // Validate the model received using 'ModelState' object inherit from 'Controller' class
        if (!ModelState.IsValid)
        {
            List<string> errorList = new();
            //foreach (var value in ModelState.Values)
            //    foreach (var error in value.Errors)
            //        errorList.Add(error.ErrorMessage);

            // Equals to the 'foreach' statement above
            errorList = ModelState.Values
                .SelectMany(value => value.Errors)
                .Select(error => error.ErrorMessage).ToList();

            string errorMessage = string.Join('\n', errorList);
            return BadRequest(errorMessage);
        }

        return Content($"{person}");
    }
}
