using Microsoft.AspNetCore.Mvc;

namespace ViewExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("home")]
        public IActionResult Index()
        {
            // this '.View()' method creates and returns an object of the view result class
            // Notice at the overload methods, if without parameter it will take the current
            //  working action method name (which is 'Index'), then take Index.cshtml to be rendered.
            //  If you pass 'abc' as the argument, it will take 'abc.cshtml' to be rendered.
            // By default the rule is that all the views should be in the Views folder,
            //  place in a sub folder with same name as the controller name.
            //  So: 'return View("abc")' -> Views/Home/Index.cshtml
            //  if the file is not in the directory, exception will be thrown
            return View();
            
            // Never be used in real world project because too lengthy
            //return new ViewResult()
            //{
            //    ViewName = "abc", // This will search for 'abc.cshtml' file for rendering the view
            //};
        }
    }
}
