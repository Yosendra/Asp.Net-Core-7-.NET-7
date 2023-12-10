using Microsoft.AspNetCore.Mvc;

namespace ControllersExample.Controllers
{
    public class HomeController
    {
        [Route("home")]
        [Route("/")]    // we can map action method to more than one path, so when root path accessed this method will be also invoked other than the path 'home'
        public string Index()   // by convention, first action method named 'Index()'
        {
            return "Hello from Index";
        }
        
        [Route("about")]
        public string About()
        {
            return "Hello from About";
        }
        
        [Route("contact-us")]   // notice the difference between 'contact-us' and 'Contact', both are independent individual things so it is possible to be different
        public string Contact()
        {
            return "Hello from Contact";
        }
    }
}
