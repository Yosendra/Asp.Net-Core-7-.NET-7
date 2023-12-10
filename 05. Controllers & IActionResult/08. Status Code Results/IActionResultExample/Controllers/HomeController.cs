
// Status Code Result
//  Status code result sends an empty response with sprecified status code. eg: 200, 400, 401, 404, 500, etc
//  Class/Type we can use to return for this:
//   StatusCodeResult   -> return new StatusCodeResult(status_code) | return StatusCode(status_code)
//    Represents response with specified status code. Used when you would like to send a specific HTTP status code as response.
//
//   BadRequestResult   -> return new BadRequestResult() | return BadRequest()
//    Represents response with HTTP status code '400 Bad Request'. Used when the request values are invalid (validation error)
//    
//   UnauthorizedResult -> return new UnauthorizedResult() | return Unauthorized()
//    Represents response with HTTP status code '401 Unauthorized'. Used when the user is unauthorized (not signed in)
//
//   NotFoundResult     -> return new NotFoundResult() | return NotFound()
//    Represents response with HTTP status code '404 Not Found'. Used when the requested information is not available at server.

using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("book")]     //  /book?bookid=1&isloggedin=true
        public IActionResult Index()
        {
            // Book id should be supplied
            if (!Request.Query.ContainsKey("bookid"))
            {
                //return Content("Book id is not supplied");
                //return new BadRequestResult();  // cannot accept error message, no need to assign status code manually
                return BadRequest("Book id is not supplied");
            }

            // Book id can't be empty
            if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            {
                return BadRequest("Book id can't be null or empty");
            }

            // Book id should be between 1 to 1000
            int bookId = Convert.ToInt32(ControllerContext.HttpContext.Request.Query["bookid"]);
            if (bookId <= 0)
            {
                return BadRequest("Book id can't be less than or equal to 0");
            }

            if (bookId > 1000)
            {
                return NotFound("Book id can't be greater than 1000");
            }

            // is 'loggedin' should be true
            if (!Convert.ToBoolean(Request.Query["isloggedin"]))
            {
                //return new UnauthorizedResult();
                return Unauthorized("User must be authenticated");
            }

            return File("/sample.pdf", "application/pdf");
        }
    }
}
