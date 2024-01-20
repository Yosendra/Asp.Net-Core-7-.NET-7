using Microsoft.AspNetCore.Mvc;

namespace CRUDExample.Controllers;

[Route("[controller]")]
public class CountryController : Controller
{
    [Route("UploadFromExcel")]
    public IActionResult UploadFromExcel()
    {
        return View();
    }
}
