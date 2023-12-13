
// [FromQuery] & [FromRoute] Attribute
//  When we want to prioritze if model binding picks the value from query string or route data
//  We can override it with '[FromQuery]' and '[FromRoute]' attributes
//  Put the attributes in action method parameter

using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers;
public class HomeController : Controller
{
    [Route("bookstore")] // url:  /bookstore?bookid=1&isloggedin=true
    // Read from query string, ignore the values come from route data
    // since we do not use [FromRoute] attribute 
    public IActionResult Index([FromQuery] int? bookId, [FromQuery] bool? isLoggedin)
    {
        if (bookId.HasValue == false)
            return BadRequest("Book id is not supplied");

        if (bookId < 0)
            return BadRequest("Book id can't be less or equal to zero");

        if (bookId > 1000)
            return NotFound("Book id can't be greater than 1000");

        if (isLoggedin is null || !isLoggedin.Value)
            return Unauthorized("User must be authenticated");

        return Content($"Book id: {bookId}", "text/plain");
    }

    [Route("bookstore/{bookId?}/{isLoggedin?}")] // url:  /bookstore/1/true
    // Read from route data, ignore the values come from query string parameter
    // since we do not use [FromQuery] attribute 
    public IActionResult Index2([FromRoute] int? bookId, [FromRoute] bool? isLoggedin)
    {
        if (bookId.HasValue == false)
            return BadRequest("Book id is not supplied");

        if (bookId < 0)
            return BadRequest("Book id can't be less or equal to zero");

        if (bookId > 1000)
            return NotFound("Book id can't be greater than 1000");

        if (isLoggedin is null || !isLoggedin.Value)
            return Unauthorized("User must be authenticated");

        return Content($"Book id: {bookId}", "text/plain");
    }
}

