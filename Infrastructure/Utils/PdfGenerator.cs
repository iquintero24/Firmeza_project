
using Firmeza.Domain.Entities;
using iTextSharp.text;
using iTextSharp.text.pdf;

namespace Infrastructure.Utils;

public class PdfGenerator
{
    
    // method for generate SaleReceipt

    public static string GenerateSaleReceipt(Sale sale)
    {
        var wwroot = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Receipt");

        if (!Directory.Exists(wwroot))
        {
            Directory.CreateDirectory(wwroot);
        }

        // name for the PDF
        var fileName = $"Receipt_{sale.ReceiptNumber}.pdf";
        var filePath = Path.Combine(wwroot, fileName);

        using (var fs = new FileStream(filePath, FileMode.Create, FileAccess.Write))
        {
            var document = new Document(PageSize.A4, 50, 50, 25, 25);
            var writer = PdfWriter.GetInstance(document, fs);
            document.Open();

            var titleFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 18);
            var subFont = FontFactory.GetFont(FontFactory.HELVETICA, 12);
            var boldTableFont = FontFactory.GetFont(FontFactory.HELVETICA_BOLD, 10);
            var bodyFont = FontFactory.GetFont(FontFactory.HELVETICA, 10);
            
            // header of PDF
            document.Add(new Paragraph("Receipt of Sale", titleFont));
            document.Add(new Paragraph($"Date: {sale.SaleDate: dd/MM/yyyy HM:mm}", subFont));
            document.Add(new Paragraph($"Receipt Number: {sale.ReceiptNumber}",subFont));
            document.Add(new  Paragraph($"Client: {sale.Customer.Name}", subFont));
            document.Add(new Paragraph(""));
            
            // table of products 
            var table = new PdfPTable(4) { WidthPercentage = 100 };
            table.AddCell(new PdfPCell(new Phrase("Product", boldTableFont)));  
            table.AddCell(new PdfPCell(new Phrase("amount", boldTableFont)));
            table.AddCell(new PdfPCell(new Phrase("Unit Price", boldTableFont)));
            table.AddCell(new PdfPCell(new Phrase("Total", boldTableFont)));

            foreach (var detail in sale.SaleDetails )
            {
                table.AddCell(new Phrase(detail.Product.Name, bodyFont));
                table.AddCell(new Phrase(detail.Quantity.ToString(), bodyFont));
                table.AddCell(new Phrase(detail.AppliedUnitPrice.ToString("C"), bodyFont));
                table.AddCell(new Phrase((detail.AppliedUnitPrice * detail.Quantity).ToString("C"), bodyFont));
            }

            document.Add(table);
            document.Add(new Paragraph(""));
            
            // totales:
            document.Add(new Paragraph($"Subtotal: {sale.Subtotal:C}", subFont));
            document.Add(new Paragraph($"IVA: {sale.Iva:C}", subFont));
            document.Add(new Paragraph($"Total: {sale.Total:C}", subFont));
            
            document.Close();
            writer.Close();
        }

        return $"/Receipt/{fileName}";

    }

}