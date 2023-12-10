
// File Results
//  File result sends the content of a file as response. eg: pdf, txt, exe, zip, etc
// 3 type of File Result:
//      VirtualFileResult   -> use this if the file is stored inside 'wwwroot' folder
//      PhysicalFileResult  -> use this if the file is stored outside 'wwwroot' folder
//      FileContentResult   -> use this if the content's file is in a form of raw byte array 

using Microsoft.AspNetCore.Mvc;
using ControllersExample.Models;

namespace ControllersExample.Controllers
{
    public class HomeController : Controller
    {
        [Route("file-download")]
        public VirtualFileResult FileDownload()
        {
            // To enable using the path, you have to activate .UseStaticFiles() middleware.
            // It search the file in 'wwwroot' folder
            
            //return new VirtualFileResult("/sample.pdf", "application/pdf
            return File("/sample.pdf", "application/pdf");
        }

        [Route("file-download2")]
        public PhysicalFileResult FileDownload2()
        {
            // PhysicalFileResult is used for file outside 'wwwroot', somewhere on project, or not necessarily part of our project
            //  that is why we write the absolute path on path parameter

            //return new PhysicalFileResult(@"c:\sample.pdf", "application/pdf")
            return PhysicalFile(@"c:\sample.pdf", "application/pdf");
        }

        [Route("file-download3")]
        public FileContentResult FileDownload3()
        {
            // FileContentResult is used when the file is in form of raw byte array

            byte[] content = System.IO.File.ReadAllBytes(@"c:\sample.pdf");

            //return new FileContentResult(content, "application/pdf");
            return File(content, "application/pdf");
        }

        [Route("person")]
        public JsonResult Person()
        {
            Person person = new()
            {
                Id = Guid.NewGuid(),
                FirstName = "James",
                LastName = "Smith",
                Age = 25
            };

            return Json(person);
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
