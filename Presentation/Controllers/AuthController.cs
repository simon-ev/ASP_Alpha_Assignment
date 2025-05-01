
using Business.Services;
using Data.Entities;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

public class AuthController : Controller
{

    //Omformaterad av chatgpt
    private readonly IAuthService _authService;
    private readonly SignInManager<UserEntity> _signInManager;
    private readonly UserManager<UserEntity> _userManager;

    public AuthController(IAuthService authService, SignInManager<UserEntity> signInManager, UserManager<UserEntity> userManager)
    {
        _authService = authService;
        _signInManager = signInManager;
        _userManager = userManager;
    }

    [Route("auth/SignUp")]
    public IActionResult SignUp()
    {
        return View();
    }

    [HttpPost]
    [Route("auth/SignUp")]
    public async Task<IActionResult> SignUp(SignUpViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        var user = new UserEntity
        {
            UserName = model.Email,
            Email = model.Email,
            FullName = model.FullName
        };

        var result = await _userManager.CreateAsync(user, model.Password);
        if (result.Succeeded)
        {
            await _signInManager.SignInAsync(user, isPersistent: false);
            return RedirectToAction("Index", "Projects");
        }

        foreach (var error in result.Errors)
        {
            ModelState.AddModelError(string.Empty, error.Description);
        }

        return View(model);
    }

    [Route("auth/Login")]
    public IActionResult Login(string returnUrl = "~/")
    {
        ViewData["ReturnUrl"] = returnUrl;
        return View();
    }

    [HttpPost]
    [Route("auth/Login")]
    public async Task<IActionResult> Login(LogInViewModel model, string returnUrl = "~/")
    {
        if (!ModelState.IsValid)
            return View(model);

        var result = await _signInManager.PasswordSignInAsync(
            model.Email,
            model.Password,
            model.IsPersistent,
            lockoutOnFailure: false);

        if (result.Succeeded)
        {
            return string.IsNullOrEmpty(returnUrl)
                ? RedirectToAction("Index", "Projects")
                : LocalRedirect(returnUrl);
        }

        ModelState.AddModelError(string.Empty, "Invalid login attempt.");
        return View(model);
    }

    [HttpPost]
    [Route("auth/Logout")]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Login");
    }
}
