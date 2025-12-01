using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Firmeza.web.Models;

namespace Firmeza.web.Controllers;

public class HomeController : Controller
{
    private readonly ILogger<HomeController> _logger;

    public HomeController(ILogger<HomeController> logger)
    {
        _logger = logger;
    }

    public IActionResult Index()
    {
        // NEW: Security Check - If the user is authenticated, redirect them to the Dashboard.
        if (User.Identity.IsAuthenticated)
        {
            // Note: We assume that if they are authenticated, they must be an Administrator
            // (since the AccountController blocks non-Administrators).
            return RedirectToAction("Index", "Dashboard"); 
        }
            
        // If not authenticated, show the public home page (with the login card).
        ViewData["Title"] = "Firmeza | Construction Supplies and Machinery Rental";
        return View();
    }

    public IActionResult Privacy()
    {
        return View();
    }

    [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
    public IActionResult Error()
    {
        return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
    }
}