using Firmeza.Domain.Entities;
using FluentAssertions;
using Infrastructure.Utils;

namespace Firmeza.Tests.Infrastructure.Utils;

public class PdfGeneratorTest
{
        [Fact]
        public void GeneratePdf_ShouldCreatePdfFile_WhenSaleIsValid()
        {
                // arrage
                var generator = new PdfGenerator();

                var sale = new Sale
                {
                        ReceiptNumber = "Test123",
                        SaleDate = DateTime.Now,
                        Customer = new Customer { Name = "Isaac" },
                        SaleDetails = new List<SaleDetail>
                        {
                                new SaleDetail
                                {
                                        Product = new Product { Name = "Producto A" },
                                        Quantity = 2,
                                        AppliedUnitPrice = 1000
                                }
                        },
                        Subtotal = 2000,
                        Iva = 380,
                        Total = 2380,
                };
                
                // act
                var relativePath = generator.GeneratePdf(sale);
                var fullPath = Path.Combine(Directory.GetCurrentDirectory(),"wwwroot", "Receipt", "Receipt_Test123.pdf");
                
                //assert 
                relativePath.Should().Contain("Receipt_Test123.pdf");
                File.Exists(fullPath).Should().BeTrue();
                
                // clean after test
                File.Delete(fullPath);
        }
}