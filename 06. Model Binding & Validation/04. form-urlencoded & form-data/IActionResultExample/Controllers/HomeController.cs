namespace IActionResultExample.Controllers;

// 'form-urlencoded' & 'form-data'
//   When you send data from form field (input text, radio, etc) in HTML.
//   Value we send in request body will be either in these two formats, 'form-urlencoded' (simple) or 'form-data' (complex)

// If you want attach files in a form, then 'form-data' is the only choice format we have to choose

using IActionResultExample.Models;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    [Route("bookstore")] // url:  /bookstore?bookid=1&isloggedin=true&author=yosi
    // try to use Postman to send request with 'POST' method, in body choose x-www-form-urlencoded
    // then include key 'bookId', value 13 and key 'author', value 'Postman' (keys same as action parameter, case-insensitive)  
    public IActionResult Index(int? bookId, // we remove the attribute to make bookId pick a value from request
                                            // body also, not forcing it to pick from query string
        [FromQuery] bool? isLoggedin,
        Book book)
    {   // Notice 'book' object, its property filled with values picked from request body from postman instead of query string
        if (bookId.HasValue == false)
            return BadRequest("Book id is not supplied");

        if (bookId < 0)
            return BadRequest("Book id can't be less or equal to zero");

        if (bookId > 1000)
            return NotFound("Book id can't be greater than 1000");

        if (isLoggedin is null || !isLoggedin.Value)
            return Unauthorized("User must be authenticated");

        return Content($"Book id: {bookId}, Book: {book}", "text/plain");
    }

    [Route("bookstore/{bookId?}/{isLoggedin?}")] // url:  /bookstore/1/true
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

