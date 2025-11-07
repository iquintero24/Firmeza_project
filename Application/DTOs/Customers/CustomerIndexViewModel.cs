using System.ComponentModel.DataAnnotations;

namespace Firmeza.Application.DTOs.Customers;

public class CustomerIndexViewModel
{
    public int Id { get; set; }

    [Display(Name = "Full Name")]
    public string Name { get; set; } = string.Empty;

    [Display(Name = "Document/ID")]
    public string Document { get; set; } = string.Empty;
        
    public string Email { get; set; } = string.Empty;

    [Display(Name = "Phone")]
    public string Phone { get; set; } = string.Empty;
}