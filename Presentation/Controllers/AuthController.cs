
using Business.Services;
using Microsoft.AspNetCore.Mvc;
using Presentation.Models;

namespace Presentation.Controllers;

public class AuthController(IAuthService authService) : Controller
{
    private readonly IAuthService _authService = authService;

    [Route("auth/SignUp")]
    public IActionResult SignUp()
    {

        return View();
    }

    [HttpPost]
    [Route("auth/SignUp")]
    public IActionResult SignUp(SignUpViewModel model)
    {
       if (!ModelState.IsValid)
            return View(model);
        
        return View();
    }

    [Route("auth/Login")]
    public IActionResult Login()
    {

        return View();
    }

    [HttpPost]
    [Route("auth/Login")]
    public IActionResult Login(LogInViewModel model)
    {
        if (!ModelState.IsValid)
            return View(model);

        return View();
    }




    //[HttpPost]
    //public async Task<IActionResult> SignUp(SignUpViewModel model)
    //{
    //    ViewBag.ErrorMessage = null;

    //    if (!ModelState.IsValid)
    //        return View(model);

    //    var result = await _authService.SignUpAsync(new SignUpFormData
    //    {
    //        FullName = model.FullName,
    //        Email = model.Email,
    //        Password = model.Password
    //    });

    //    if (result.Succeeded)
    //    {
    //        return RedirectToAction("SignIn", "Auth");
    //    }

    //    ViewBag.ErrorMessage = result.Error;
    //    return View(model);
    //}

    //public IActionResult SignIn(string returnUrl = "~/")
    //{
    //    ViewBag.ReturnUrl = returnUrl;
    //    return View();
    //}

    //[HttpPost]
    //public async Task<IActionResult> SignIn(LogInViewModel model, string returnUrl = "~/")
    //{
    //    ViewBag.ErrorMessage = null;
    //    ViewBag.ReturnUrl = returnUrl;

    //    if (!ModelState.IsValid)
    //        return View(model);

    //    var result = await _authService.SignInAsync(new SignInFormData
    //    {
    //        Email = model.Email,
    //        Password = model.Password
    //    });

    //    if (result.Succeeded)
    //    {
    //        return LocalRedirect(returnUrl);
    //    }

    //    ViewBag.ErrorMessage = result.Error;
    //    return View(model);
    //}

}
