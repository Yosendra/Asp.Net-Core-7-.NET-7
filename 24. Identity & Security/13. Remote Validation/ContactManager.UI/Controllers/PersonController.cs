﻿using CRUDExample.Filters;
using CRUDExample.Filters.ActionFilters;
using CRUDExample.Filters.AuthorizationFilters;
using CRUDExample.Filters.ResourceFilters;
using CRUDExample.Filters.ResultFilters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Rotativa.AspNetCore;
using Rotativa.AspNetCore.Options;
using ServiceContracts;
using ServiceContracts.DTO;
using ServiceContracts.Enums;

namespace CRUDExample.Controllers;

[Route("[controller]/[action]")]
[ResponseHeaderFilterFactory("Key-From-Controller", "Value-From-Controller", 3)]
[TypeFilter(typeof(PersonAlwaysRunResultFilter))]
public class PersonController : Controller
{
    #region Fields
    private readonly IPersonService _personService;
    private readonly ICountryService _countryService;
    private readonly ILogger<PersonController> _logger;
    #endregion

    #region Constructor
    public PersonController(
        IPersonService personService,
        ICountryService countryService,
        ILogger<PersonController> logger)
    {
        _personService = personService;
        _countryService = countryService;
        _logger = logger;
    }
    #endregion

    [Route("/")]
    [ServiceFilter(typeof(PersonListActionFilter), Order = 4)]
    [ResponseHeaderFilterFactory("Key-From-Action", "Value-From-Action", 1)]
    [TypeFilter(typeof(PersonListResultFilter))]
    [SkipFilter]
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

    public async Task<IActionResult> Create()
    {
        var countries = await _countryService.GetAllCountries();
        ViewBag.Countries = countries.Select(c => new SelectListItem { Text = c.Name, Value = c.Id.ToString() });

        return View();
    }

    [HttpPost]
    [TypeFilter(typeof(PersonCreateAndEditPostActionFilter))]
    [TypeFilter(typeof(FeatureDisableResourceFilter), Arguments = new object[] { false, })]
    public async Task<IActionResult> Create(PersonAddRequest requestModel)
    {
        await _personService.AddPerson(requestModel);
        return RedirectToAction("Index", "Person");
    }

    [Route("{id}")]
    [TypeFilter(typeof(TokenResultFilter))]
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
    [Route("{id}")]
    [TypeFilter(typeof(TokenAuthorizationFilter))]
    public async Task<IActionResult> Edit(PersonUpdateRequest requestModel)
    {
        PersonResponse? person = await _personService.GetPersonById(requestModel.Id);
        if (person == null)
            return RedirectToAction("Index");

        if (ModelState.IsValid)
        {
            await _personService.UpdatePerson(requestModel);
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

    [Route("{id}")]
    public async Task<IActionResult> Delete(Guid id)
    {
        PersonResponse? person = await _personService.GetPersonById(id);
        if (person == null)
            return RedirectToAction("Index");

        return View(person);
    }

    [HttpPost]
    [Route("{id}")]
    public async Task<IActionResult> Delete(PersonUpdateRequest updatePersonModel)
    {
        PersonResponse? person = await _personService.GetPersonById(updatePersonModel.Id);
        if (person == null)
            return RedirectToAction("Index");

        await _personService.DeletePerson(person.Id);
        
        return RedirectToAction("Index");
    }

    public async Task<IActionResult> PersonPDF()
    {
        var persons = await _personService.GetAllPersons();
        return new ViewAsPdf(persons)
        {
            PageMargins = new Margins() { Top = 20, Right = 20, Bottom = 20, Left = 20, },
            PageOrientation = Orientation.Landscape,
        };
    }

    public async Task<IActionResult> PersonCSV()
    {
        MemoryStream memoryStream = await _personService.GetPersonCSV();
        return File(memoryStream, "application/octet-stream", "person.csv");
    }

    public async Task<IActionResult> PersonExcel()
    {
        MemoryStream memoryStream = await _personService.GetPersonExcel();
        return File(memoryStream, "application/vnd.openxmlformats-officedocument.spreadsheetml.sheet", "person.xlsx");
    }
}
