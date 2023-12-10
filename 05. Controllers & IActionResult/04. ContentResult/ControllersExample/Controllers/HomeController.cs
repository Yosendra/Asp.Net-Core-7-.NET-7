
// ContentResult
//  ContentResult can represent any type of response, based on the specified MIME type.
//  MIME type represents type of the content such as text/plain, text/html, application/json, application/xml, application/pdf, etc

using Microsoft.AspNetCore.Mvc;

namespace ControllersExample.Controllers
{
    public class HomeController : Controller    // Inherit 'Controller' class
    {
        [Route("home")]
        [Route("/")]
        public ContentResult Index()    // notice the return type 'ContentResult', it is not best practice using string in return type
        {
            //return new ContentResult()
            //{   // Define the content and content-type in the response
            //    // content will take place in response body, meanwhile content-type in response header
            //    Content = "Hello from Index",
            //    ContentType = "text/plain",
            //};

            // or you can use this Content() to make the response,
            // but controller must inherit class from 'Controller'
            //return Content("Hello from Index", "text/plain");

            // it is possible to return html content, but for this, it is not the recommended way
            return Content("<h1>Welcome</h1> <h3>Hello from Index</h3>", "text/html");
        }
        
        [Route("about")]
        public string About()
        {
            return "Hello from About";
        }
        
        [Route("contact-us")]
        public string Contact()
        {
            return "Hello from Contact";
        }
    }
}
