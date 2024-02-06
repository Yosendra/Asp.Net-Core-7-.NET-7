using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers;

public class HomeController : Controller
{
    [Route("Error")]
    public IActionResult Error()
    {
        var cxceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        if (cxceptionHandlerPathFeature != null && cxceptionHandlerPathFeature.Error != null)
        {
            ViewBag.ErrorMessage = cxceptionHandlerPathFeature.Error.Message;
        }

        return View();
    }
}
