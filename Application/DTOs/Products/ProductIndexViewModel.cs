using System.ComponentModel.DataAnnotations;

namespace Firmeza.Application.DTOs.Products;

public class ProductIndexViewModel
{
    public int Id { get; set; }

    [Display(Name = "Product Name")]
    public string Name { get; set; } = string.Empty;

    // Mapea a Product.UnitPrice
    [Display(Name = "Unit Price")] 
    [DataType(DataType.Currency)]
    public decimal UnitPrice { get; set; } 

    [Display(Name = "Current Stock")]
    public int Stock { get; set; }
}