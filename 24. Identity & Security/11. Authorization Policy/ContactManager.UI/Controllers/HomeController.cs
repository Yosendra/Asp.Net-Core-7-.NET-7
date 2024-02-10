using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers;

public class HomeController : Controller
{
    [AllowAnonymous]    // allow this action method to be accessed without login
    [Route("Error")]
    public IActionResult Error()
    {
        var exceptionHandlerPathFeature = HttpContext.Features.Get<IExceptionHandlerPathFeature>();
        
        if (exceptionHandlerPathFeature != null && exceptionHandlerPathFeature.Error != null)
            ViewBag.ErrorMessage = exceptionHandlerPathFeature.Error.Message;

        return View();
    }
}
