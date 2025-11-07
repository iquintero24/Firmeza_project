using Firmeza.Application.DTOs;
using Firmeza.Domain.Entities;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;



namespace Firmeza.web.Controllers;

public class AccountController : Controller
{
    // injections of dependencies for identity 
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly UserManager<ApplicationUser> _userManager;

    // constructor for account controller
    public AccountController(SignInManager<ApplicationUser> signInManager, UserManager<ApplicationUser> userManager)
    {
        _signInManager = signInManager;
        _userManager = userManager;
    }

    // method for redirecting to view login
    [HttpGet]
    public IActionResult Login(string? returnUrl = null)
    {
        // valid that user auth not return login if be auth
        if (User.Identity.IsAuthenticated) return RedirectToAction("Index", "Dashboard");
        var model = new LoginViewModel{ ReturnUrl = returnUrl };
        return View(model);
    }
    
    // AccountController.cs (The [HttpPost] Login Method)

    [HttpPost]
    [AllowAnonymous]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Login(LoginViewModel model)
    {
        // 1. Validate the model data (required fields)
        if (ModelState.IsValid)
        {
            // 2. Attempt to authenticate the user
            var result = await _signInManager.PasswordSignInAsync(
                model.Email, 
                model.Password, 
                model.RememberMe, 
                lockoutOnFailure: false); 

            // 3. Check for failed authentication (invalid credentials)
            if (!result.Succeeded)
            {
                // IMPORTANT: Add error message to be displayed in the view
                ModelState.AddModelError(string.Empty, "Invalid login attempt. Please check your email and password.");
                return View(model);
            }

            // 4. Authentication was successful, now check for the required role
            var user = await _userManager.FindByEmailAsync(model.Email);
        
            // Ensure the user exists and has the Administrator role
            if (user != null && await _userManager.IsInRoleAsync(user, "Administrator"))
            {
                // Login successful and is Administrator, redirect to Dashboard
                // NOTE: The LocalRedirect performs the POST-Redirect-GET pattern (PRG)
                return LocalRedirect(model.ReturnUrl ?? "/Dashboard/Index");
            }
            else
            {
                // User authenticated BUT IS NOT an Administrator (Role Restriction - Task 4)
                await _signInManager.SignOutAsync();
            
                // Display a specific message for role denial
                ModelState.AddModelError(string.Empty, "Access denied. Only administrators are authorized to use this panel.");
                return View(model);
            }
        }
    
        // If we reach here, model validation failed (e.g., empty fields)
        return View(model);
    }
    
    // post method for logout
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Logout()
    {
        await _signInManager.SignOutAsync();
        return RedirectToAction("Index", "Home");
    }
    

}