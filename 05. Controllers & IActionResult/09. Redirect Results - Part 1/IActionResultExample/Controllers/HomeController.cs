
// Redirect Result
//  Redirect result sends either HTTP 302 or 301 response to the browser, in order to redirect to a specific action or url.
//  eg. redirecting from 'action1' to 'action2'

using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        // suppose when user request coming to this path, user will redidirected to '/store/books'

        [Route("bookstore")] // url:  /bookstore?bookid=1&isloggedin=true
        public IActionResult Index()
        {
            // Book id should be supplied
            if (!Request.Query.ContainsKey("bookid"))
                return BadRequest("Book id is not supplied");

            // Book id can't be empty
            if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
                return BadRequest("Book id can't be null or empty");

            // Book id should be between 1 to 1000
            int bookId = Convert.ToInt32(ControllerContext.HttpContext.Request.Query["bookid"]);
            if (bookId <= 0)
                return BadRequest("Book id can't be less than or equal to 0");

            if (bookId > 1000)
                return NotFound("Book id can't be greater than 1000");

            // is 'loggedin' should be true
            if (!Convert.ToBoolean(Request.Query["isloggedin"]))
                return Unauthorized("User must be authenticated");

            // Instead of returning the result, we redirect the user to another path
            // look at intellisense for the information of each argument
            // url: store/books

            return new RedirectToActionResult("Books", "Store", null);
        }
    }
}
