using ContactManager.Core.Domain.IdentityEntities;
using ContactManager.Core.Dto;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.UI.Controllers;

[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;

    public AccountController(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        // if registerDto contain not valid data
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage);
            return View(registerDto);
        }

        // To do: Store user registration details into Identity database
        ApplicationUser user = new()
        {
            Email = registerDto.Email,
            UserName = registerDto.Email,
            PersonName = registerDto.PersonName,
            PhoneNumber = registerDto.Phone,
        };

        IdentityResult result = await _userManager.CreateAsync(user, registerDto.Password);
        if (result.Succeeded)
        {
            return RedirectToAction("Index", "Person");
        }
        else
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("Register", error.Description);

            return View(registerDto);
        }

    }
}
