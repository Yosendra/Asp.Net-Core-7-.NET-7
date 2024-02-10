﻿using ContactManager.Core.Domain.IdentityEntities;
using ContactManager.Core.Dto;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Infrastructure;

namespace ContactManager.UI.Controllers;

[AllowAnonymous]    // allow all action methods under this controller can be accessed without login
[Route("[controller]/[action]")]
public class AccountController : Controller
{
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;

    public AccountController(
        UserManager<ApplicationUser> userManager, 
        SignInManager<ApplicationUser> signInManager)
    {
        _userManager = userManager;
        _signInManager = signInManager;
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
    public async Task<IActionResult> Login(LoginDto loginDto)
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
}