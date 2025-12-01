using System.ComponentModel.DataAnnotations;

namespace Firmeza.Application.DTOs.Customers;

public class CustomerCreateViewModel
{
    [Required(ErrorMessage = "The customer's name is mandatory.")]
    [StringLength(100)]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [Required(ErrorMessage = "The document/identification is mandatory.")]
    [StringLength(20)]
    [Display(Name = "Document/ID")]
    public string Document { get; set; } = string.Empty;
        
    [Required(ErrorMessage = "The email address is mandatory.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(150)]
    // Nota: Este campo se usará como el UserName en Identity
    public string Email { get; set; } = string.Empty;
        
    [Phone(ErrorMessage = "Invalid phone format.")]
    [StringLength(15)]
    [Display(Name = "Phone")]
    public string Phone { get; set; } = string.Empty;
    
    // ---------------------------------------------
    // --- CAMPOS AÑADIDOS PARA IDENTITY (Login) ---
    // ---------------------------------------------
    
    [Required(ErrorMessage = "The password is mandatory.")]
    [DataType(DataType.Password)]
    [Display(Name = "Password")]
    public string Password { get; set; } = string.Empty;
        
    [DataType(DataType.Password)]
    [Display(Name = "Confirm Password")]
    // Esta validación asegura que el usuario escriba la misma contraseña dos veces
    [Compare("Password", ErrorMessage = "The password and confirmation do not match.")]
    public string ConfirmPassword { get; set; } = string.Empty;
}