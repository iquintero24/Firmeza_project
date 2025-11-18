namespace Firmeza.Application.DTOs.Sales;

public class SaleIndexViewModel
{
    public int Id { get; set; }
    public DateTime SaleDate { get; set; }
    public string ReceiptNumber { get; set; } = string.Empty;
    public string CustomerName { get; set; } = string.Empty;
    public decimal Total { get; set; }
    public string? PdfUrl { get; set; }
}