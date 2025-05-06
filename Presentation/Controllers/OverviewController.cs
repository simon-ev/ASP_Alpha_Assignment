using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Presentation.Controllers;

[Authorize]
[Route("admin/overview")]
public class OverviewController : Controller
{

    public IActionResult Index()
    {
        return View();
    }


    //[Route("admin/overview")]
    //public IActionResult Index()
    //{
    //    // skriven av Chatgpt 
    //    if (User.Identity?.IsAuthenticated == true)
    //    {
    //        return RedirectToAction("Index", "BackOffice");
    //    }
    //    else
    //    {
    //        return RedirectToAction("Login", "Auth");
    //    }
    //}
}
