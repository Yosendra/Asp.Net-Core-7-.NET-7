namespace IActionResultExample.Controllers;

// Models
//  Models is a class that represents structure of data (as properties) that 
//  you would like to receive from the request and/or send to the response.
//  It also known as POCO (Plain Old CLR Objects).

using IActionResultExample.Models;
using Microsoft.AspNetCore.Mvc;

public class HomeController : Controller
{
    [Route("bookstore")] // url:  /bookstore?bookid=1&isloggedin=true&author=yosi
    public IActionResult Index([FromQuery] int? bookId, 
        [FromQuery] bool? isLoggedin,
        Book book) // If you access the url above, 'Book' model will be instantiated and
                   // its property 'BookId' will be filled with value 1 because it has same name 'BookId'
                   // with query string parameter.
    {
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

