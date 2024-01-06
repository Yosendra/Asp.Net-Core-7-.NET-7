using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers;

[Route("[controller]")]
public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly ICountryService _countryService;

    public PersonController(IPersonService personService, ICountryService countryService)
    {
        _personService = personService;
        _countryService = countryService;
    }

    [Route("/")]
    [Route("[action]")]
    public IActionResult Index(string searchBy,
                               string? keyword,
                               string sortBy = nameof(PersonResponse.Name),
                               SortOrderEnum sortOrder = SortOrderEnum.ASC)
    {
        ViewBag.SearchFields = new Dictionary<string, string>()
        {
            {nameof(PersonResponse.Name), "Name"},
            {nameof(PersonResponse.Email), "Email"},
            {nameof(PersonResponse.DateOfBirth), "Date of Birth"},
            {nameof(PersonResponse.Gender), "Gender"},
            {nameof(PersonResponse.CountryId), "Country"},
            {nameof(PersonResponse.Address), "Address"},
        };

        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentKeyword = keyword;

        var persons = _personService.GetFilteredPersons(searchBy, keyword);
        persons = _personService.GetSortedPersons(persons, sortBy, sortOrder);

        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder.ToString();

        return View(persons);
    }

    [HttpGet]
    [Route("[action]")]
    public IActionResult Create()
    {
        var countries = _countryService.GetAllCountries();
        ViewBag.Countries = countries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

        return View();
    }

    [HttpPost]
    [Route("[action]")]
    public IActionResult Create(PersonAddRequest requestModel)
    {
        if (!ModelState.IsValid)
        {
            List<CountryResponse> countries = _countryService.GetAllCountries();
            ViewBag.Countries = countries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
            
            return View();
        }

        PersonResponse personAdded = _personService.AddPerson(requestModel);

        return RedirectToAction("Index", "Person");
    }

    [HttpGet]
    [Route("[action]/{id}")]
    public IActionResult Edit(Guid id)
    {
        PersonResponse person = _personService.GetPersonById(id);
        if (person == null)
        {
            return RedirectToAction("Index");
        }

        var countries = _countryService.GetAllCountries();
        ViewBag.Countries = countries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

        PersonUpdateRequest model = person.ToPersonUpdateRequest();
        return View(model);
    }

    [HttpPost]
    [Route("[action]/{id}")]
    public IActionResult Edit(PersonUpdateRequest updatePersonModel)
    {
        PersonResponse? person = _personService.GetPersonById(updatePersonModel.Id);
        if (person == null)
        {
            return RedirectToAction("Index");
        }

        if (ModelState.IsValid)
        {
            _personService.UpdatePerson(updatePersonModel);
            return RedirectToAction("Index");
        }
        else
        {
            List<CountryResponse> countries = _countryService.GetAllCountries();
            ViewBag.Countries = countries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
            return View();
        }
    }
}
