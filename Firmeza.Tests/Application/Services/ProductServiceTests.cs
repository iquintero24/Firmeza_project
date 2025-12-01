using FluentAssertions;
using Moq;
using Firmeza.Application.Implemetations;
using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Entities;
using Firmeza.Application.DTOs.Products;

namespace Firmeza.Tests.Application.Services
{
    public class ProductServiceTests
    {
        private readonly Mock<IProductRepository> _repoMock;
        private readonly ProductService _service;

        public ProductServiceTests()
        {
            _repoMock = new Mock<IProductRepository>();
            _service = new ProductService(_repoMock.Object);
        }

        // ============================================================
        // 1. GET ALL PRODUCTS
        // ============================================================
        [Fact]
        public async Task GetAllProductsAsync_ShouldReturnAllProducts()
        {
            _repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Product>
                {
                    new Product { Id = 1, Name = "Prod 1", UnitPrice = 1000, Stock = 5 },
                    new Product { Id = 2, Name = "Prod 2", UnitPrice = 2000, Stock = 10 }
                });

            var result = await _service.GetAllProductsAsync();

            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Prod 1");
        }

        // ============================================================
        // 2. GET PRODUCT FOR EDIT
        // ============================================================
        [Fact]
        public async Task GetProductForEditAsync_ShouldReturnProduct_WhenExists()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync(new Product
                {
                    Id = 1,
                    Name = "Laptop",
                    Description = "Nice laptop",
                    UnitPrice = 3500,
                    Stock = 10
                });

            var result = await _service.GetProductForEditAsync(1);

            result.Should().NotBeNull();
            result!.Id.Should().Be(1);
            result.Name.Should().Be("Laptop");
        }

        [Fact]
        public async Task GetProductForEditAsync_ShouldReturnNull_WhenNotExists()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1))
                .ReturnsAsync((Product?)null);

            var result = await _service.GetProductForEditAsync(1);

            result.Should().BeNull();
        }

        // ============================================================
        // 3. CREATE PRODUCT
        // ============================================================
        [Fact]
        public async Task CreateProductAsync_ShouldCreateProduct()
        {
            var model = new ProductCreateViewModel
            {
                Name = "Mouse",
                Description = "Wireless",
                UnitPrice = 50,
                Stock = 100
            };

            _repoMock.Setup(r => r.AddAsync(It.IsAny<Product>()))
                .ReturnsAsync(new Product
                {
                    Id = 10,
                    Name = "Mouse",
                    UnitPrice = 50,
                    Stock = 100
                });

            var result = await _service.CreateProductAsync(model);

            result.Should().NotBeNull();
            result.Id.Should().Be(10);
            result.Name.Should().Be("Mouse");

            _repoMock.Verify(r => r.AddAsync(It.IsAny<Product>()), Times.Once);
        }

        // ============================================================
        // 4. UPDATE PRODUCT
        // ============================================================
        [Fact]
        public async Task UpdateProductAsync_ShouldUpdate_WhenExists()
        {
            var existing = new Product
            {
                Id = 1,
                Name = "Old",
                Description = "Old desc",
                UnitPrice = 100,
                Stock = 5
            };

            var updated = new ProductEditViewModel
            {
                Id = 1,
                Name = "New",
                Description = "New desc",
                UnitPrice = 150,
                Stock = 8
            };

            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
            _repoMock.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(true);

            var result = await _service.UpdateProductAsync(updated);

            result.Should().BeTrue();
            existing.Name.Should().Be("New");
            existing.Description.Should().Be("New desc");
        }

        [Fact]
        public async Task UpdateProductAsync_ShouldReturnFalse_WhenNotExists()
        {
            _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync((Product?)null);

            var result = await _service.UpdateProductAsync(new ProductEditViewModel { Id = 1 });

            result.Should().BeFalse();
        }

        // ============================================================
        // 5. DELETE PRODUCT
        // ============================================================
        [Fact]
        public async Task DeleteProductAsync_ShouldCallRepositoryDelete()
        {
            _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

            var result = await _service.DeleteProductAsync(1);

            result.Should().BeTrue();
            _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
        }
    }
}
