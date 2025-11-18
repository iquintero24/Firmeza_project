using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using Firmeza.Application.Interfaces;
using Firmeza.Domain.Entities;
using Firmeza.Infrastructure.Persistence;
using Microsoft.AspNetCore.Identity;
using Microsoft.IdentityModel.Tokens;

namespace Firmeza.Application.Implemetations;

public class TokenService: ITokenService
{
    
    private readonly UserManager<ApplicationUser> _userManager;

    public TokenService(UserManager<ApplicationUser> userManager)
    {
        _userManager = userManager;
    }
    
    public async Task<string> CreateTokenAsync(ApplicationUser user)
    {
        var SecretKey = Environment.GetEnvironmentVariable("TOKEN_JWT");

        if (string.IsNullOrEmpty(SecretKey))
        {
            throw new Exception("TOKEN_JWT not configuration in the file .env");
        }

        var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecretKey));
        var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

        var claims = new List<Claim>
        {
            new Claim(JwtRegisteredClaimNames.Sub, user.Id.ToString()),
            new Claim(JwtRegisteredClaimNames.UniqueName, user.UserName ?? ""),
            new Claim(ClaimTypes.Email, user.Email ?? ""),
            new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString())
        };
        
        // get the roles of entity
        var roles = await _userManager.GetRolesAsync(user);
        foreach (var role in roles ) claims.Add(new Claim(ClaimTypes.Role, role));

            var token = new JwtSecurityToken(claims: claims,expires: DateTime.UtcNow.AddHours(2),signingCredentials:creds);
            
            return new JwtSecurityTokenHandler().WriteToken(token);
            
    }
}