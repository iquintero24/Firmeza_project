using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Firmeza.Domain.Entities;

public class Sale
{
    // Primary Key
    [Key]
    public int Id { get; set; }

    // Sale registration date
    [DataType(DataType.DateTime)]
    public DateTime SaleDate { get; set; } = DateTime.Now;

    // Sale/Receipt number
    [StringLength(20)]
    public string ReceiptNumber { get; set; }  = string.Empty;

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Subtotal { get; set; }

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Iva { get; set; } // The receipt must include IVA 

    [Column(TypeName = "decimal(18, 2)")]
    public decimal Total { get; set; } // The receipt must include the Total 

    // Foreign Key to Customer (N:1 Relationship)
    [Required]
    public int CustomerId { get; set; }

    [ForeignKey("CustomerId")]
    public required Customer Customer { get; set; }

    // Relationship: A sale has many sale details (1:N)
    public required ICollection<SaleDetail> SaleDetails { get; set; }
}