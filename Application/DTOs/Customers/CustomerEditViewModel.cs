using System.ComponentModel.DataAnnotations;

namespace Firmeza.Application.DTOs.Customers;

public class CustomerEditViewModel
{
    [Required]
    public int Id { get; set; } 

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
    public string Email { get; set; } = string.Empty;
        
    [Phone(ErrorMessage = "Invalid phone format.")]
    [StringLength(15)]
    [Display(Name = "Phone")]
    public string Phone { get; set; } = string.Empty;
}