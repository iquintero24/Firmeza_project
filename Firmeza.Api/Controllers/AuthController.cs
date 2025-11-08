using Firmeza.Application.DTOs;
using Firmeza.Application.DTOs.Customers;
using Firmeza.Application.Interfaces;
using Firmeza.Domain.Entities;
using Firmeza.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AuthController : ControllerBase
{
    private readonly ICustomerService _customerService;
    private readonly UserManager<ApplicationUser> _userManager;
    private readonly SignInManager<ApplicationUser> _signInManager;
    private readonly ITokenService _tokenService;

    public AuthController(ICustomerService customerService, UserManager<ApplicationUser> userManager, SignInManager<ApplicationUser> signInManager, ITokenService tokenService)
    {
        _customerService = customerService;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CustomerCreateViewModel model)
    {
        try
        {
            // create the client 
            var newCustomer = await _customerService.CreateCustomerAsync(model);

            // assign rol "client" in identity 
            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
            }

            // new token JWT
            var token = await _tokenService.CreateTokenAsync(user!);

            return Ok(new
            {
                message = "Successfully registered user",
                token,
                customer = newCustomer
            });
        }
        catch (InvalidOperationException e)
        {
            return BadRequest(new { message = e.Message });
        }
        catch (Exception ex)
        {
            return StatusCode(500, new { message = "Internal error of register client", detail = ex.Message });
        }
    }

    // login of client 
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        var user = await _userManager.FindByEmailAsync(request.Email);
        if(user == null)
            return Unauthorized(new {message = "user not found."});
        
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return Unauthorized(new { message = "Credentials invalid"});

        var token = await _tokenService.CreateTokenAsync(user);
        return Ok(new { token });
    }
    
    
    
}