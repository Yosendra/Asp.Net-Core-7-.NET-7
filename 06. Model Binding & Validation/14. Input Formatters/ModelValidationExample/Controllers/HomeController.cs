using Microsoft.AspNetCore.Mvc;
using ModelValidationExample.Models;

// [FromBody]
//  Enables the input formatters to read data from request body as json, xml, or custom only
//
// Case: what if the request body is in XML format?
//
// InputFormatter -> internal classes which are used to transform or convert the request body into a model object 
// Need to add service for reading XML format. Look at 'Program.cs'

namespace ModelValidationExample.Controllers;

public class HomeController : Controller
{
    // Request from Postman with POST method to url '/register'
    // Request Body (xml)
    //   <Person>
    //      <Name>Scott</Name>
    //      <Email>scott @example.com</Email>
    //   </Person>

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
