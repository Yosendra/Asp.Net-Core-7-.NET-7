using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers;

//[Route("person")]   // this acts as the common prefix for all the action routes of the same controller

[Route("[controller]")]    // Route token, automatically pick the action method name. It equal to the route above
public class PersonController : Controller
{
    private readonly IPersonService _personService;
    private readonly ICountryService _countryService;

    public PersonController(IPersonService personService, ICountryService countryService)
    {
        _personService = personService;
        _countryService = countryService;   // inject
    }

    // url: /person/index
    [Route("/")]
    //[Route("person/index")]
    //[Route("index")]     // because we have define the prefix, we can write the url like this. It equals as comment above.

    [Route("[action]")]    // Route token, automatically pick the action method name. It equal to the route above
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

    // url: person/create
    [HttpGet]
    //[Route("person/create")]
    //[Route("create")]   // because we have define the prefix, we can write the url like this. It equals as comment above.

    [Route("[action]")]    // Route token, automatically pick the action method name. It equal to the route above
    public IActionResult Create()
    {
        var countries = _countryService.GetAllCountries();
        ViewBag.Countries = countries;

        return View();
    }

    // url: person/create
    [HttpPost]
    //[Route("person/create")]
    //[Route("create")]    // because we have define the prefix, we can write the url like this. It equals as comment above.

    [Route("[action]")]    // Route token, automatically pick the action method name. It equal to the route above
    public IActionResult Create(PersonAddRequest requestModel)
    {
        if (!ModelState.IsValid)
        {
            List<CountryResponse> countries = _countryService.GetAllCountries();
            ViewBag.Countries = countries;
            ViewBag.Errors = ModelState.Values.SelectMany(v => v.Errors)
                                              .Select(e => e.ErrorMessage)
                                              .ToList();
            
            return View();
        }

        PersonResponse personAdded = _personService.AddPerson(requestModel);

        return RedirectToAction("Index", "Person");
    }
}
