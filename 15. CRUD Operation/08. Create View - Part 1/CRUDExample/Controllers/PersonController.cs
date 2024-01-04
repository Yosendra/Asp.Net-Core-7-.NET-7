using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers;

public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly ICountryService _countryService;

    public PersonController(IPersonService personService, ICountryService countryService)
    {
        _personService = personService;
        _countryService = countryService;   // inject
    }

    [Route("person/index")]
    [Route("/")]
    // Notice, the parameter name is same as form element name
    // in query parameter
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

        // To persist the searchBy and keyword value in the index view
        ViewBag.CurrentSearchBy = searchBy;
        ViewBag.CurrentKeyword = keyword;

        //var persons = _personService.GetAllPersons();
        var persons = _personService.GetFilteredPersons(searchBy, keyword);

        // Sorting list
        persons = _personService.GetSortedPersons(persons, sortBy, sortOrder);

        // To persist the sortBy and sortOrder value in the index view
        ViewBag.CurrentSortBy = sortBy;
        ViewBag.CurrentSortOrder = sortOrder.ToString();

        return View(persons);
    }

    [HttpGet]
    [Route("person/create")]
    public IActionResult Create()
    {
        var countries = _countryService.GetAllCountries();
        ViewBag.Countries = countries;

        return View();
    }
}
