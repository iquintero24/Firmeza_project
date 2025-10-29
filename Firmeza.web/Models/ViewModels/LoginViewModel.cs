using System.ComponentModel.DataAnnotations;

namespace Firmeza.web.Models.ViewModels;

public class LoginViewModel
{
    [Required(ErrorMessage = "The email address is required.")]
    [EmailAddress]
    public string Email { get; set; } = string.Empty;

    [Required(ErrorMessage = "The password is required.")]
    [DataType(DataType.Password)]
    public string Password { get; set; } = string.Empty;

    [Display(Name = "¿Remember?")]
    public bool RememberMe { get; set; }
        
    // To redirect to the original URL after login
    public string? ReturnUrl { get; set; }
}