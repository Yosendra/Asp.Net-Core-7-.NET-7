
// JsonResult
// Represents an object in Javascript Object Notation (JSON) format. eg. {"firstname": "James", "lastName": "Smith", "age": 25}

// POCO class -> Plain Old CLR Object
//  contains a set of properties. It defines what data you would like to store in an object

using Microsoft.AspNetCore.Mvc;
using ControllersExample.Models;

namespace ControllersExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("person")]
        public JsonResult Person()      // Returning json text to response
        {
            Person person = new()
            {
                Id = Guid.NewGuid(),
                FirstName = "James",
                LastName = "Smith",
                Age = 25
            };

            //return new JsonResult(person);  // You can return the created JsonResult object
            return Json(person);              // or use 'Json()' inherited method from controller class for simplicity and cleaner
        }

        [Route("home")]
        [Route("/")]
        public ContentResult Index()
        {
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
