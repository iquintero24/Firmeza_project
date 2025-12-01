using System.ComponentModel.DataAnnotations;

namespace Firmeza.Application.DTOs.Sales
{
    public class SaleEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int CustomerId { get; set; }

        [Required]
        [DataType(DataType.DateTime)]
        public DateTime SaleDate { get; set; }

        [Required]
        [StringLength(20)]
        public string ReceiptNumber { get; set; } = string.Empty;

        [Required]
        public decimal Subtotal { get; set; }

        [Required]
        public decimal Iva { get; set; }

        [Required]
        public decimal Total { get; set; }
        
        public List<SaleDetailEditViewModel>? SaleDetails { get; set; }
    }

    public class SaleDetailEditViewModel
    {
        [Required]
        public int Id { get; set; }

        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public decimal AppliedUnitPrice { get; set; }
    }
}