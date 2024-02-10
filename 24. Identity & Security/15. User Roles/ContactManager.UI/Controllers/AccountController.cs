using ContactManager.Core.Domain.IdentityEntities;
using ContactManager.Core.Dto;
using ContactManager.Core.Enums;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace ContactManager.UI.Controllers;

[AllowAnonymous]
[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly RoleManager<ApplicationRole> _roleManager;

    public AccountController(
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        RoleManager<ApplicationRole> roleManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
        _roleManager = roleManager;
    }

    public IActionResult Register()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Register(RegisterDto registerDto)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage);
            return View(registerDto);
        }

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
            // check the status of radio button selected is valid
            if (registerDto.UserType is UserTypeOptions.Admin or UserTypeOptions.User)
            {
                // check the role is available
                ApplicationRole role = await _roleManager.FindByNameAsync(registerDto.UserType.ToString());
                if (role == null)
                {
                    // create the new role
                    role = new() { Name = registerDto.UserType.ToString() };
                    await _roleManager.CreateAsync(role);
                }

                // Assign the new user into the role
                await _userManager.AddToRoleAsync(user, role.Name);
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Person");
        }
        else
        {
            foreach (IdentityError error in result.Errors)
                ModelState.AddModelError("Register", error.Description);

            return View(registerDto);
        }
    }

    public IActionResult Login()
    {
        return View();
    }

    [HttpPost]
    public async Task<IActionResult> Login(LoginDto loginDto, string? returnUrl)
    {
        if (!ModelState.IsValid)
        {
            ViewBag.Errors = ModelState.Values.SelectMany(m => m.Errors).Select(e => e.ErrorMessage);
            return View(loginDto);
        }

        string? userName = loginDto.Email;
        string? password = loginDto.Password;
        var result = await _signInManager.PasswordSignInAsync(userName, password, false, false);
        if (result.Succeeded)
        {
            if (!string.IsNullOrEmpty(returnUrl) && Url.IsLocalUrl(returnUrl))
                return LocalRedirect(returnUrl);

            return RedirectToAction("Index", "Person");
        }
        else
        {
            ModelState.AddModelError("Login", "Invalid email or password");
            return View(loginDto);
        }
    }

    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Person");
    }

    public async Task<IActionResult> IsEmailAlreadyRegistered(string email)
    {
        ApplicationUser user = await _userManager.FindByEmailAsync(email);
        return Json(user == null);
    }
}
