using Firmeza.Application.DTOs;
using Firmeza.Application.DTOs.Customers;
using Firmeza.Application.Interfaces;
using Firmeza.Domain.Entities;
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

    public AuthController(
        ICustomerService customerService,
        UserManager<ApplicationUser> userManager,
        SignInManager<ApplicationUser> signInManager,
        ITokenService tokenService)
    {
        _customerService = customerService;
        _userManager = userManager;
        _signInManager = signInManager;
        _tokenService = tokenService;
    }

    // ======================================================
    // REGISTER
    // ======================================================
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] CustomerCreateViewModel model)
    {
        try
        {
            var newCustomer = await _customerService.CreateCustomerAsync(model);

            var user = await _userManager.FindByEmailAsync(model.Email);
            if (user != null)
            {
                await _userManager.AddToRoleAsync(user, "Customer");
            }

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

    // ======================================================
    // LOGIN (DEVUELVE TOKEN + CUSTOMER)
    // ======================================================
    [HttpPost("login")]
    public async Task<IActionResult> Login([FromBody] LoginRequest request)
    {
        // 1. Buscar usuario Identity
        var user = await _userManager.FindByEmailAsync(request.Email);
        if (user == null)
            return Unauthorized(new { message = "User not found." });

        // 2. Validar contraseña
        var result = await _signInManager.CheckPasswordSignInAsync(user, request.Password, false);
        if (!result.Succeeded)
            return Unauthorized(new { message = "Invalid credentials" });

        // 3. Crear token
        var token = await _tokenService.CreateTokenAsync(user);

        // 4. Obtener Customer asociado
        var customer = await _customerService.GetCustomerByIdentityUserIdAsync(user.Id);

        return Ok(new
        {
            token,
            customer
        });
    }
}
