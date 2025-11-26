using FluentAssertions;
using Moq;
using Firmeza.Application.Implemetations;
using Firmeza.Domain.Interfaces;
using Firmeza.Domain.Entities;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.Tests.Application.Services
{
    public class CustomerServiceTests
    {
        private readonly Mock<ICustomerRepository> _repoMock;
        private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
        private readonly CustomerService _service;

        public CustomerServiceTests()
        {
            // Mock del repositorio
            _repoMock = new Mock<ICustomerRepository>();

            // Mock del UserManager (truco típico para tests)
            _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                Mock.Of<IUserStore<ApplicationUser>>(),
                null, null, null, null, null, null, null, null
            );

            // Servicio real usando los mocks
            _service = new CustomerService(_repoMock.Object, _userManagerMock.Object);
        }

        // -------------------------------
        // 🟢 TEST 1: GetAllCustomersAsync
        // -------------------------------
        [Fact]
        public async Task GetAllCustomersAsync_ShouldReturnCustomers()
        {
            // Arrange
            _repoMock.Setup(r => r.GetAllAsync())
                .ReturnsAsync(new List<Customer>
                {
                    new Customer { Id = 1, Name = "Test 1", Email = "a@test.com", Document = "111" },
                    new Customer { Id = 2, Name = "Test 2", Email = "b@test.com", Document = "222" }
                });

            // Act
            var result = await _service.GetAllCustomersAsync();

            // Assert
            result.Should().HaveCount(2);
            result.First().Name.Should().Be("Test 1");
        }
    }
}