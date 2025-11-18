using Firmeza.Domain.Entities;

namespace Firmeza.Application.Interfaces;

public interface ITokenService
{
    /// <summary>
    /// Generate a JWT for specified user, their roles a claims
    /// </summary>
    /// <param name="user"></param>
    /// <returns></returns>
    Task<string> CreateTokenAsync(ApplicationUser user);
}