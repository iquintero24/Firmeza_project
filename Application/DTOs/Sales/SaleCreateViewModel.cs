using System.ComponentModel.DataAnnotations;

namespace Firmeza.Application.DTOs.Sales
{
    public class SaleDetailCreateViewModel
    {
        [Required]
        public int ProductId { get; set; }

        [Required]
        [Range(1, int.MaxValue)]
        public int Quantity { get; set; }

        [Required]
        public decimal AppliedUnitPrice { get; set; }
    }

    public class SaleCreateViewModel
    {
        [Required]
        public int CustomerId { get; set; }

        [Required]
        public List<SaleDetailCreateViewModel> SaleDetails { get; set; } = new();

        [Required]
        public decimal Subtotal { get; set; }

        [Required]
        public decimal Iva { get; set; }

        [Required]
        public decimal Total { get; set; }
    }
}