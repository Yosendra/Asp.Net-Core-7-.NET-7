using Microsoft.AspNetCore.Mvc;

namespace ControllersExample.Controllers
{
    // 1. You need to register HomeController as a service class, in order to participate in dependency injection
    // 2. Enable the routing for this method
    
    // Service class means a reusable class. A class which performs a specific functionality
    //  without having direct relationship with UI.
    // In ASP.NET, the controllers are also treated as services.
    // Generally we create services for business logic

    public class HomeController
    {
        // This called 'Attribut Routing', register the action method to a path
        // Whenever request come to this path, this method will be invoked
        [Route("/sayhello")]
        public string method1()
        {
            return "Hello from method1";    // this return statement will become part of response
        }
    }
}
