using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firmeza.web.Data.Entities;

public class Product
{
    // Primary Key
    [Key]
    public int Id { get; set; }

    [Required(ErrorMessage = "The product name is mandatory.")]
    [StringLength(100)]
    public string Name { get; set; } = string.Empty;

    [StringLength(500)]
    public string Description { get; set; }  = string.Empty;


    [Required(ErrorMessage = "The unit price is mandatory.")]
    [Range(0.01, 999999.99, ErrorMessage = "The price must be positive.")]
    [DataType(DataType.Currency)]
    [Column(TypeName = "decimal(18, 2)")] 
    [Display(Name = "Unit Price")]
    public decimal UnitPrice { get; set; } 

    // Requirement: Stock (Quantity)
    [Required(ErrorMessage = "Stock is mandatory.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock cannot be negative.")]
    public int Stock { get; set; }

    // Relationship: A product can be in many sale details (1:N)
    public required ICollection<SaleDetail> SaleDetails { get; set; }
}