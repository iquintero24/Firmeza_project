using System.ComponentModel.DataAnnotations;

namespace Firmeza.web.Data.Entities;

public class Customer
{
    // Primary Key
    [Key]
    public int Id { get; set; }

    // Requirement: Mandatory name validation
    [Required(ErrorMessage = "The customer's name is mandatory.")]
    [StringLength(100)]
    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    // Requirement: Document/ID validation
    [Required(ErrorMessage = "The document/identification is mandatory.")]
    [StringLength(20)]
    public string Document { get; set; }  = string.Empty;
    
    [Required(ErrorMessage = "The email address is mandatory.")]
    [EmailAddress(ErrorMessage = "Invalid email format.")]
    [StringLength(150)]
    public string Email { get; set; }  = string.Empty;
    
    [Phone(ErrorMessage = "Invalid phone format.")]
    [StringLength(15)]
    [Display(Name = "Phone")]
    public string Phone { get; set; } = string.Empty;

    // Relationship: A customer can have many sales (1:N)
    public ICollection<Sale> Sales { get; set; } = new List<Sale>();
}