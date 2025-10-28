using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firmeza.web.Data.Entities;

public class SaleDetail
{
    // Primary Key
    [Key]
    public int Id { get; set; }

    // Foreign Key to Sale (N:1 Relationship)
    [Required]
    public int SaleId { get; set; }

    [ForeignKey("SaleId")]
    public required Sale Sale { get; set; }

    // Foreign Key to Product (N:1 Relationship)
    [Required]
    public int ProductId { get; set; }

    [ForeignKey("ProductId")]
    public required Product Product { get; set; }
    
    [Required(ErrorMessage = "Quantity is mandatory.")]
    [Range(1, int.MaxValue, ErrorMessage = "Quantity must be greater than zero.")]
    public int Quantity { get; set; }

    // Price at which the product was sold (important for historical integrity)
    [Required]
    [Column(TypeName = "decimal(18, 2)")]
    [Display(Name = "Applied Unit Price")]
    public decimal AppliedUnitPrice { get; set; }
}