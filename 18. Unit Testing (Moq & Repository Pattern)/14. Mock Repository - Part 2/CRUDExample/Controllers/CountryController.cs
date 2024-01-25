using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CRUDExample.Controllers;

[Route("[controller]")]
public class CountryController : Controller
{
    private readonly ICountryService _countryService;

    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }

    [HttpGet]
    [Route("[action]")]
    public IActionResult UploadFromExcel()
    {
        return View();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> UploadFromExcel(IFormFile excelFile) // notice the type, and the name must be same as the name in the form
    {
        if (excelFile == null || excelFile.Length == 0)
        {
            ViewBag.ErrorMessage = "Please select an excel file";
            return View();
        }

        if (!Path.GetExtension(excelFile.FileName).Equals(".xlsx", StringComparison.OrdinalIgnoreCase))
        {
            ViewBag.ErrorMessage = "Unsupported file. '.xlsx' file is expected";
            return View();
        }

        int countCountryInserted = await _countryService.UploadCountryFromExcelFile(excelFile);
        
        ViewBag.Message = $"{countCountryInserted} countries uploaded";
        
        return View();
    }
}
