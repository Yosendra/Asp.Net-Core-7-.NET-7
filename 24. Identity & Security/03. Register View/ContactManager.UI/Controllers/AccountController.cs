using ContactManager.Core.Dto;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.UI.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public IActionResult Register(RegisterDto registerDto)
    {
        // To do: Store user registration details into Identity database
        return RedirectToAction("Index", "Person");
    }
}
