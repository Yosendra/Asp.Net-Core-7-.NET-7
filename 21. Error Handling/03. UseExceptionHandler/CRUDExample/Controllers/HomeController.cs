using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers;

public class HomeController : Controller
{
    [Route("Error")]
    public IActionResult Error()
    {
        // this will give us the current working exception details
        // Every exception will be contained in this Feature collection when it happen
        // So you can access the exception through this collection anywhere
        var cxceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (cxceptionHandlerPathFeature != null && cxceptionHandlerPathFeature.Error != null)
        {
            ViewBag.ErrorMessage = cxceptionHandlerPathFeature.Error.Message;
        }

        return View();          // put the page at this directory View/Shared/Error because we want it to be shared
    }
}
