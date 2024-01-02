using Microsoft.AspNetCore.Mvc;
using ServiceContracts;

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
    public IActionResult Index()
    {
        var persons = _personService.GetAllPersons();

        return View(persons);
    }
}
