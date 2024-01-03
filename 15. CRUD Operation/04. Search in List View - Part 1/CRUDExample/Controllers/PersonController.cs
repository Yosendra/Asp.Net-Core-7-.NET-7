﻿using Microsoft.AspNetCore.Mvc;
using ServiceContracts;
using ServiceContracts.DTO;

namespace CRUDExample.Controllers;

public class PersonController : Controller
{
    private readonly IPersonService _personService;

    public PersonController(IPersonService personService)
    {
        _personService = personService;
    }

    [Route("person/index")]
    [Route("/")]
    // Notice, the parameter name is same as form element name
    // in query parameter
    public IActionResult Index(string searchBy, string? keyword) 
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

        var persons = _personService.GetAllPersons();

        return View(persons);
    }
}
