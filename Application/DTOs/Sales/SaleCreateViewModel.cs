using System.ComponentModel.DataAnnotations;
using Firmeza.Application.DTOs.Customers;
using Firmeza.Application.DTOs.Products;

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
        // --------- Customer selection ---------
        [Required]
        public int CustomerId { get; set; }

        public List<CustomerIndexViewModel> Customers { get; set; } = new();


        // --------- Product selection ---------
        public List<ProductIndexViewModel> Products { get; set; } = new();

        [Required]
        public List<SaleDetailCreateViewModel> SaleDetails { get; set; } = new();


        // --------- Calculated fields ---------
        [Required]
        public decimal Subtotal { get; set; }

        [Required]
        public decimal Iva { get; set; }

        [Required]
        public decimal Total { get; set; }
    }
}