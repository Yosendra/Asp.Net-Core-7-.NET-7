using Microsoft.AspNetCore.Mvc; // namespace for '[Controller]' attribute

// builder.Services.AddControllers()
//  Adds all controllers as services in the IServiceCollection.
//  So that, they can be accessed when a specific endpoint needs it.

// app.MapControllers()
//  Add all action methods as endpoints.
//  So that, no need of using 'UseEndpoints()' method for adding action method as endpoint

// Reponsibility of Controllers
// 1. Reading requests
//      Extracting data value from request such as query string parameters, request body,
//      request cookies, request headers, etc
// 2. Validation
//      Validate incoming request details (query string parameters, request body,
//      request cookies, request headers, etc)
// 3. Invoking models
//      Calling business logic methods. Generally business operations are available as 'services'
// 4. Preparing response
//      Choosing what kind of response has to be sent to the client & also preparing the response (action result)

namespace ControllersExample.Controllers
{
    [Controller]
    public class HomeController   // If you don't want to suffix the class name with '-Controller' which means only 'Home' you have to use attribut above
    {
        [Route("home")]
        [Route("/")]
        public string Index() 
        { 
            return "Hello from Index";
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
