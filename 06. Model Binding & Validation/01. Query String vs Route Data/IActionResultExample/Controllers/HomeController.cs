
// Model Binding
//  Model Binding is a feature of asp.net that reads values from
//  http requests and pass them as arguments to the action method.

//  HTTP request -> Routing -> Model Binding -------------> Controller
//							    Form fields					 Action1
//							    Request body
//							    Route data
//							    Query string parameters

// Query String vs Route Data

using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers;
public class HomeController : Controller
{
    [Route("bookstore")] // url:  /bookstore?bookid=1&isloggedin=true
    // We have to define the parameter for the action method which we want to use 'Model Binding'
    // This is will automatically read from 'Query String Parameter' 
    // The parameters name is same as the name of variable in request we want to read the value, in this example are 'bookId' and 'isLoggedin'
    // When we access the url above, model binding is already happen.
    public IActionResult Index(int? bookId, bool? isLoggedin)   // Here we use 'Parameter Query String'
    {
        // Problem : it is too lengthy wrting 'Request.Query.ContainsKey("key")'
        //  every time we want to want to read values in request object
         
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
    // By priority, asp.net prioritize 'Route Data' rather than 'Query String Parameter' if both are together in the url
    // you can try by accessing url '/bookstore/10/false?bookId=1&isloggedin=true', it will pick the value from 'Route Data'
    public IActionResult Index2(int? bookId, bool? isLoggedin)   // Here we use 'Route Data'
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

