using Microsoft.AspNetCore.Mvc;
using ModelValidationExample.Models;

// [FromBody]
//  Enables the input formatters to read data from request body as json, xml, or custom only

namespace ModelValidationExample.Controllers;

public class HomeController : Controller
{
    // Request from Postman with POST method to url '/register'
    // Request Body (json)
    //   {
    //      "Name": "scott",
    //      "Email": "scott@example.com"
    //   }

    [Route("register")]
    public IActionResult Index([FromBody] Person person) // This enable to parse json format text and convert it to 'Person' model object
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
