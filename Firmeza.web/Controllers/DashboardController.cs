using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.web.Controllers;

// the controller is protected
[Authorize(Roles = "Administrator")]
public class DashboardController: Controller
{

    public IActionResult Index()
    {
        ViewData["Title"] = "Dashboard overview";
        return View();
    }
}