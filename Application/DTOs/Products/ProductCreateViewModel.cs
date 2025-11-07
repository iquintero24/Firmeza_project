using System.ComponentModel.DataAnnotations;

namespace Firmeza.Application.DTOs.Products;

public class ProductCreateViewModel
{
        [Required(ErrorMessage = "The Name field is required.")]
        [StringLength(100, ErrorMessage = "The Name must not exceed 100 characters.")]
        [Display(Name = "Product Name")]
        public string Name { get; set; } = string.Empty;

        [Display(Name = "Product Description")]
        [DataType(DataType.MultilineText)]
        [StringLength(500)] // Coincide con el límite de la entidad
        public string Description { get; set; } = string.Empty;

        // Mapea a Product.UnitPrice
        [Required(ErrorMessage = "The Unit Price field is required.")]
        [Range(0.01, 999999.99, ErrorMessage = "The Price must be positive.")] 
        [DataType(DataType.Currency)]
        [Display(Name = "Unit Price")]
        public decimal UnitPrice { get; set; } // Nombre ajustado

        [Required(ErrorMessage = "The Stock field is required.")]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be a positive number.")]
        [Display(Name = "Initial Stock")]
        public int Stock { get; set; }
}