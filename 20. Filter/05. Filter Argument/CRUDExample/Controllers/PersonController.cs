using CRUDExample.Filters.ActionFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers;

[Route("[controller]")]
public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly ICountryService _countryService;
    private readonly ILogger<PersonController> _logger;

    public PersonController(IPersonService personService, ICountryService countryService, ILogger<PersonController> logger)
    {
        _personService = personService;
        _countryService = countryService;
        _logger = logger;
    }

    [Route("/")]
    [Route("[action]")]
    [TypeFilter(typeof(PersonListActionFilter))]
    [TypeFilter(typeof(ResponseHeaderActionFilter), Arguments = new object[] { "X-Custom-Key", "Custom-Value" })]     // notice this
    public async Task<IActionResult> Index(
        string searchBy,
        string? keyword,
        string sortBy = nameof(PersonResponse.Name),
        SortOrderEnum sortOrder = SortOrderEnum.ASC)
    {
        _logger.LogInformation("Index action method of PersonController");
        _logger.LogDebug(
            $"{nameof(searchBy)}: {searchBy}, " +
            $"{nameof(keyword)}: {keyword}, " +
            $"{nameof(sortBy)}: {sortBy}, " +
            $"{nameof(sortOrder)}: {sortOrder} ");

        var persons = await _personService.GetFilteredPersons(searchBy, keyword);
        persons = await _personService.GetSortedPersons(persons, sortBy, sortOrder);

        return View(persons);
    }

    [HttpGet]
    [Route("[action]")]
    public async Task<IActionResult> Create()
    {
        var countries = await _countryService.GetAllCountries();
        ViewBag.Countries = countries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

        return View();
    }

    [HttpPost]
    [Route("[action]")]
    public async Task<IActionResult> Create(PersonAddRequest requestModel)
    {
        if (!ModelState.IsValid)
        {
            List<CountryResponse> countries = await _countryService.GetAllCountries();
            ViewBag.Countries = countries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();

            return View(requestModel);
        }

        PersonResponse personAdded = await _personService.AddPerson(requestModel);

        return RedirectToAction("Index", "Person");
    }

    [HttpGet]
    [Route("[action]/{id}")]
    public async Task<IActionResult> Edit(Guid id)
    {
        PersonResponse? person = await _personService.GetPersonById(id);
        if (person == null)
            return RedirectToAction("Index");

        var countries = await _countryService.GetAllCountries();
        ViewBag.Countries = countries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

        PersonUpdateRequest model = person.ToPersonUpdateRequest();
        return View(model);
    }

    [HttpPost]
    [Route("[action]/{id}")]
    public async Task<IActionResult> Edit(PersonUpdateRequest updatePersonModel)
    {
        PersonResponse? person = await _personService.GetPersonById(updatePersonModel.Id);
        if (person == null)
            return RedirectToAction("Index");

        if (ModelState.IsValid)
        {
            await _personService.UpdatePerson(updatePersonModel);
            return RedirectToAction("Index");
        }
        else
        {
            List<CountryResponse> countries = await _countryService.GetAllCountries();
            ViewBag.Countries = countries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            ViewBag.Errors = ModelState.Values
                .SelectMany(v => v.Errors)
                .Select(e => e.ErrorMessage)
                .ToList();
            
            return View(person.ToPersonUpdateRequest());
        }
    }

    [HttpGet]
    [Route("[action]/{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        PersonResponse? person = await _personService.GetPersonById(id);
        if (person == null)
            return RedirectToAction("Index");

        return View(person);
    }

    [HttpPost]
    [Route("[action]/{id}")]
    public async Task<IActionResult> Delete(PersonUpdateRequest updatePersonModel)
    {
        PersonResponse? person = await _personService.GetPersonById(updatePersonModel.Id);
        if (person == null)
            return RedirectToAction("Index");

        await _personService.DeletePerson(person.Id);
        
        return RedirectToAction("Index");
    }

    [Route("[action]")]
    public async Task<IActionResult> PersonPDF()
    {
        var persons = await _personService.GetAllPersons();

        return new ViewAsPdf(persons)
        {
            PageMargins = new Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20, },
            PageOrientation = Orientation.Landscape,
        };
    }

    [Route("[action]")]
    public async Task<IActionResult> PersonCSV()
    {
        MemoryStream memoryStream = await _personService.GetPersonCSV();
        return File(memoryStream, "application/octet-stream", "person.csv");
    }

    [Route("[action]")]
    public async Task<IActionResult> PersonExcel()
    {
        MemoryStream memoryStream = await _personService.GetPersonExcel();
        return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "person.xlsx");
    }
}
