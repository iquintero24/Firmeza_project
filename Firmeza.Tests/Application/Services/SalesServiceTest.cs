using Firmeza.Application.DTOs.Sales;
using Firmeza.Application.Implemetations;
using Firmeza.Application.Interfaces;
using Firmeza.Domain.Entities;
using Firmeza.Domain.Interfaces;
using FluentAssertions;
using Moq;
using Microsoft.AspNetCore.Hosting;

namespace Firmeza.Tests.Application.Services
{
    public class SaleServiceTests
    {
        private readonly Mock<ISaleRepository> _saleRepo;
        private readonly Mock<ICustomerRepository> _customerRepo;
        private readonly Mock<IProductRepository> _productRepo;
        private readonly Mock<IEmailService> _emailService;
        private readonly Mock<IPdfGenerator> _pdfGenerator;
        private readonly Mock<IWebHostEnvironment> _env;
        private readonly SaleService _service;

        public SaleServiceTests()
        {
            _saleRepo = new Mock<ISaleRepository>();
            _customerRepo = new Mock<ICustomerRepository>();
            _productRepo = new Mock<IProductRepository>();
            _emailService = new Mock<IEmailService>();
            _pdfGenerator = new Mock<IPdfGenerator>();
            _env = new Mock<IWebHostEnvironment>();

            _env.Setup(e => e.WebRootPath).Returns(Directory.GetCurrentDirectory());

            _service = new SaleService(
                _saleRepo.Object,
                _customerRepo.Object,
                _productRepo.Object,
                _emailService.Object,
                _pdfGenerator.Object,
                _env.Object
            );
        }

        // ============================================================
        // CREATE SALE - SUCCESS
        // ============================================================
        [Fact]
        public async Task CreateSaleAsync_ShouldCreateSale_WhenValid()
        {
            var customer = new Customer { 
                Id = 1, 
                Name = "Isaac", 
                Email = "test@mail.com" 
            };

            var product = new Product { 
                Id = 2, 
                Name = "PC Gamer", 
                Stock = 10, 
                UnitPrice = 2000 
            };

            _customerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);
            _productRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(product);

            _pdfGenerator.Setup(g => g.GeneratePdf(It.IsAny<Sale>()))
                .Returns("/Receipt/test.pdf");

            _saleRepo.Setup(r => r.AddAsync(It.IsAny<Sale>()))
                .ReturnsAsync(new Sale
                {
                    Id = 99,
                    CustomerId = customer.Id,
                    Customer = customer,
                    SaleDate = DateTime.UtcNow,
                    ReceiptNumber = "ABC123",
                    Subtotal = 2000,
                    Iva = 380,
                    Total = 2380,
                    SaleDetails = new List<SaleDetail>()
                });

            var model = new SaleCreateViewModel
            {
                CustomerId = 1,
                Subtotal = 2000,
                Iva = 380,
                Total = 2380,
                SaleDetails = new List<SaleDetailCreateViewModel>
                {
                    new SaleDetailCreateViewModel
                    {
                        ProductId = 2,
                        Quantity = 1,
                        AppliedUnitPrice = 2000
                    }
                }
            };

            var result = await _service.CreateSaleAsync(model);

            result.Id.Should().Be(99);
            result.CustomerName.Should().Be("Isaac");

            _saleRepo.Verify(r => r.AddAsync(It.IsAny<Sale>()), Times.Once);
            _pdfGenerator.Verify(g => g.GeneratePdf(It.IsAny<Sale>()), Times.Once);
            _emailService.Verify(e => e.SendEmailWithPdf(
                customer.Email, It.IsAny<string>(), It.IsAny<string>(), It.IsAny<string>()
            ), Times.Once);

            product.Stock.Should().Be(9);
        }

        // ============================================================
        // CREATE SALE - CUSTOMER NOT FOUND
        // ============================================================
        [Fact]
        public async Task CreateSaleAsync_ShouldThrow_WhenCustomerDoesNotExist()
        {
            _customerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Customer?)null);

            var model = new SaleCreateViewModel 
            { 
                CustomerId = 1,
                SaleDetails = new List<SaleDetailCreateViewModel>()
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateSaleAsync(model));
        }

        // ============================================================
        // CREATE SALE - PRODUCT NOT FOUND
        // ============================================================
        [Fact]
        public async Task CreateSaleAsync_ShouldThrow_WhenProductDoesNotExist()
        {
            var customer = new Customer { 
                Id = 1,
                Name = "Isaac"
            };

            _customerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);
            _productRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync((Product?)null);

            var model = new SaleCreateViewModel
            {
                CustomerId = 1,
                SaleDetails = new List<SaleDetailCreateViewModel>
                {
                    new SaleDetailCreateViewModel 
                    { 
                        ProductId = 2, 
                        Quantity = 1 
                    }
                }
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateSaleAsync(model));
        }

        // ============================================================
        // CREATE SALE - NOT ENOUGH STOCK
        // ============================================================
        [Fact]
        public async Task CreateSaleAsync_ShouldThrow_WhenNotEnoughStock()
        {
            var customer = new Customer { 
                Id = 1,
                Name = "Isaac"
            };

            var product = new Product { 
                Id = 2, 
                Stock = 0 
            };

            _customerRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);
            _productRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(product);

            var model = new SaleCreateViewModel
            {
                CustomerId = 1,
                SaleDetails = new List<SaleDetailCreateViewModel>
                {
                    new SaleDetailCreateViewModel 
                    { 
                        ProductId = 2, 
                        Quantity = 1 
                    }
                }
            };

            await Assert.ThrowsAsync<InvalidOperationException>(() =>
                _service.CreateSaleAsync(model));
        }

        // ============================================================
        // GET ALL SALES
        // ============================================================
        [Fact]
        public async Task GetAllSalesAsync_ShouldReturnList()
        {
            _saleRepo.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Sale>
                {
                    new Sale { 
                        Id = 1, 
                        CustomerId = 1,
                        Customer = new Customer { Id = 1, Name = "Isaac" }, 
                        Total = 100,
                        SaleDetails = new List<SaleDetail>()
                    },
                    new Sale { 
                        Id = 2, 
                        CustomerId = 2,
                        Customer = new Customer { Id = 2, Name = "David" }, 
                        Total = 200,
                        SaleDetails = new List<SaleDetail>()
                    }
                });

            var result = await _service.GetAllSalesAsync();

            result.Should().HaveCount(2);
        }

        // ============================================================
        // GET SALE FOR EDIT
        // ============================================================
        [Fact]
        public async Task GetSaleForEditAsync_ShouldReturnSale()
        {
            _saleRepo.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Sale
                {
                    Id = 1,
                    CustomerId = 5,
                    Customer = new Customer { Id = 5, Name = "Test" },
                    SaleDetails = new List<SaleDetail>()
                });

            var result = await _service.GetSaleForEditAsync(1);

            result!.Id.Should().Be(1);
            result.CustomerId.Should().Be(5);
        }

        // ============================================================
        // DELETE SALE
        // ============================================================
        [Fact]
        public async Task DeleteSaleAsync_ShouldDelete()
        {
            var sale = new Sale
            {
                Id = 1,
                CustomerId = 1,
                Customer = new Customer { Id = 1, Name = "Test" },
                SaleDetails = new List<SaleDetail>
                {
                    new SaleDetail { ProductId = 2, Quantity = 1, Product = new Product { Id = 2 } }
                }
            };

            var product = new Product { Id = 2, Stock = 5 };

            _saleRepo.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(sale);
            _productRepo.Setup(r => r.GetByIdAsync(2)).ReturnsAsync(product);
            _saleRepo.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _service.DeleteSaleAsync(1);

            result.Should().BeTrue();
            product.Stock.Should().Be(6);
        }

        // ============================================================
        // GET SALES BY CUSTOMER
        // ============================================================
        [Fact]
        public async Task GetSalesByCustomerAsync_ShouldReturnSales()
        {
            _saleRepo.Setup(r => r.GetSalesByCustomerAsync(5))
                .ReturnsAsync(new List<Sale>
                {
                    new Sale { 
                        Id = 1, 
                        CustomerId = 5,
                        Customer = new Customer { Id = 5, Name = "Isaac" }, 
                        Total = 100,
                        SaleDetails = new List<SaleDetail>()
                    }
                });

            var result = await _service.GetSalesByCustomerAsync(5);

            result.Should().HaveCount(1);
            result.First().CustomerName.Should().Be("Isaac");
        }
    }
}
