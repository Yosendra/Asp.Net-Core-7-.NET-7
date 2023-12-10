
// IActionController
//  is the parent interface for all action result classes such as ContentResult, JsonResult, RedirectResult, StatusCodeResult, ViewResult, etc
//  By mentioning the return type as IActionResult, you can return either of the subtype of IActionResult

using Microsoft.AspNetCore.Mvc;

namespace IActionResultExample.Controllers
{
    public class HomeController : Controller
    {
        // We want to create app where the path is '/book' with
        // query parameter 'isloggedin' must be true
        // query parameter 'bookid' must be in range 1 to 1000
        // if either one is invalid, then display approriate error message

        // This is 'IActionResult' take place. Notice to each return statement we write,
        // there are different types of object we return by 'Content()' and 'File()'. They are 'ContentResult' and 'VirtualFileResult'
        // We bridge them by returning both of their parent, that is 'IActionResult' interface

        [Route("book")] //Url:  /book?bookid=1&isloggedin=true
        public IActionResult Index()
        {
            // Book id should be supplied
            if (!Request.Query.ContainsKey("bookid"))
            {
                Response.StatusCode = 400;
                return Content("Book id is not supplied");
            }

            // Book id can't be empty
            if (string.IsNullOrEmpty(Convert.ToString(Request.Query["bookid"])))
            {
                Response.StatusCode = 400;
                return Content("Book id can't be null or empty");
            }

            // Book id should be between 1 to 1000
            // Request object can also be accessed through 'Request.Query["key"]' directly
            // But the actual is 'ControllerContext.HttpContext.Request.Query["key"]' 
            int bookId = Convert.ToInt32(ControllerContext.HttpContext.Request.Query["bookid"]);
            if (bookId <= 0)
            {
                Response.StatusCode = 400;
                return Content("Book id can't be less than or equal to 0");
            }

            if (bookId > 1000)
            {
                Response.StatusCode = 400;
                return Content("Book id can't be greater than 1000");
            }

            // is 'loggedin' should be true
            if (!Convert.ToBoolean(Request.Query["isloggedin"]))
            {
                Response.StatusCode = 401;  // Unauthorized, user must be logged in
                return Content("User must be authenticated");
            }

            return File("/sample.pdf", "application/pdf");
        }
    }
}
