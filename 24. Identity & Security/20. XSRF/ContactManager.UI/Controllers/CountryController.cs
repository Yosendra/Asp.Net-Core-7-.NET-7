using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

namespace CRUDExample.Controllers;

[Route("[controller]/[action]")]
public class CountryController : Controller
{
    #region Fields
    private readonly ICountryService _countryService;
    #endregion

    #region Constructor
    public CountryController(ICountryService countryService)
    {
        _countryService = countryService;
    }
    #endregion


    public IActionResult UploadFromExcel()
    {
        return View();
    }


    [HttpPost]
    public async Task<IActionResult> UploadFromExcel(IFormFile excelFile)
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
