    using FluentAssertions;
    using Moq;
    using Firmeza.Application.Implemetations;
    using Firmeza.Domain.Interfaces;
    using Firmeza.Domain.Entities;
    using Microsoft.AspNetCore.Identity;
    using Firmeza.Application.DTOs.Customers;

    namespace Firmeza.Tests.Application.Services
    {
        public class CustomerServiceTests
        {
            private readonly Mock<ICustomerRepository> _repoMock;
            private readonly Mock<UserManager<ApplicationUser>> _userManagerMock;
            private readonly CustomerService _service;

            public CustomerServiceTests()
            {
                _repoMock = new Mock<ICustomerRepository>();

                _userManagerMock = new Mock<UserManager<ApplicationUser>>(
                    Mock.Of<IUserStore<ApplicationUser>>(),
                    null, null, null, null, null, null, null, null
                );

                _service = new CustomerService(_repoMock.Object, _userManagerMock.Object);
            }

            // ---------------------------------------------------------
            // 1. GET ALL CUSTOMERS
            // ---------------------------------------------------------
            [Fact]
            public async Task GetAllCustomersAsync_ShouldReturnCustomers()
            {
                _repoMock.Setup(r => r.GetAllAsync())
                    .ReturnsAsync(new List<Customer>
                    {
                        new Customer { Id = 1, Name = "Test 1", Email = "a@test.com", Document = "111" },
                        new Customer { Id = 2, Name = "Test 2", Email = "b@test.com", Document = "222" }
                    });

                var result = await _service.GetAllCustomersAsync();

                result.Should().HaveCount(2);
            }

            // ---------------------------------------------------------
            // 2. GET CUSTOMER FOR EDIT
            // ---------------------------------------------------------
            [Fact]
            public async Task GetCustomerForEditAsync_ShouldReturnCustomer_WhenExists()
            {
                _repoMock.Setup(r => r.GetByIdAsync(1))
                    .ReturnsAsync(new Customer
                    {
                        Id = 1,
                        Name = "Isaac",
                        Document = "123",
                        Email = "test@mail.com",
                        Phone = "300123"
                    });

                var result = await _service.GetCustomerForEditAsync(1);

                result.Should().NotBeNull();
                result!.Id.Should().Be(1);
            }

            // ---------------------------------------------------------
            // 3. CREATE CUSTOMER
            // ---------------------------------------------------------
            [Fact]
            public async Task CreateCustomerAsync_ShouldCreateCustomer_WhenValid()
            {
                var model = new CustomerCreateViewModel
                {
                    Name = "Isaac",
                    Document = "123",
                    Email = "test@mail.com",
                    Phone = "300123",
                    Password = "Pass123$"
                };

                _repoMock.Setup(r => r.GetAllAsync())
                    .ReturnsAsync(new List<Customer>());

                _userManagerMock.Setup(u => u.CreateAsync(It.IsAny<ApplicationUser>(), model.Password))
                    .ReturnsAsync(IdentityResult.Success);

                _repoMock.Setup(r => r.AddAsync(It.IsAny<Customer>()))
                    .ReturnsAsync(new Customer { Id = 10, Name = "Isaac" });

                var result = await _service.CreateCustomerAsync(model);

                result.Should().NotBeNull();
                result.Id.Should().Be(10);

                _userManagerMock.Verify(u => u.CreateAsync(It.IsAny<ApplicationUser>(), model.Password), Times.Once);
            }

            // ---------------------------------------------------------
            // 4. UPDATE CUSTOMER
            // ---------------------------------------------------------
            [Fact]
            public async Task UpdateCustomerAsync_ShouldUpdate_WhenExists()
            {
                var existing = new Customer
                {
                    Id = 1,
                    Name = "Old",
                    Email = "old@mail.com",
                    Document = "111",
                    Phone = "123"
                };

                var updated = new CustomerEditViewModel
                {
                    Id = 1,
                    Name = "New",
                    Email = "new@mail.com",
                    Document = "111",
                    Phone = "456"
                };

                _repoMock.Setup(r => r.GetAllAsync()).ReturnsAsync(new List<Customer> { existing });
                _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(existing);
                _repoMock.Setup(r => r.UpdateAsync(existing)).ReturnsAsync(true);

                var result = await _service.UpdateCustomerAsync(updated);

                result.Should().BeTrue();
                existing.Name.Should().Be("New");
                existing.Email.Should().Be("new@mail.com");
            }

            // ---------------------------------------------------------
            // 5. DELETE CUSTOMER
            // ---------------------------------------------------------
            [Fact]
            public async Task DeleteCustomerAsync_ShouldDelete_WhenCustomerExists()
            {
                var customer = new Customer
                {
                    Id = 1,
                    IdentityUserId = "user123",
                    Sales = new List<Sale>()
                };

                _repoMock.Setup(r => r.GetByIdAsync(1)).ReturnsAsync(customer);
                _repoMock.Setup(r => r.DeleteAsync(1)).ReturnsAsync(true);

                _userManagerMock.Setup(u => u.FindByIdAsync("user123"))
                    .ReturnsAsync(new ApplicationUser());

                _userManagerMock.Setup(u => u.DeleteAsync(It.IsAny<ApplicationUser>()))
                    .ReturnsAsync(IdentityResult.Success);

                var result = await _service.DeleteCustomerAsync(1);

                result.Should().BeTrue();
                _repoMock.Verify(r => r.DeleteAsync(1), Times.Once);
            }

            // ---------------------------------------------------------
            // 6. DUPLICATE VALIDATION
            // ---------------------------------------------------------
            [Fact]
            public async Task IsDocumentOrEmailDuplicateAsync_ShouldReturnTrue_WhenDuplicateExists()
            {
                _repoMock.Setup(r => r.GetAllAsync())
                    .ReturnsAsync(new List<Customer>
                    {
                        new Customer { Id = 2, Document = "123", Email = "aaa@mail.com" }
                    });

                var result = await _service.IsDocumentOrEmailDuplicateAsync(1, "123", "new@mail.com");

                result.Should().BeTrue();
            }

            // ---------------------------------------------------------
            // 7. GET CUSTOMER BY IDENTITY USER ID
            // ---------------------------------------------------------
            [Fact]
            public async Task GetCustomerByIdentityUserIdAsync_ShouldReturnCustomer_WhenExists()
            {
                _repoMock.Setup(r => r.GetAllAsync())
                    .ReturnsAsync(new List<Customer>
                    {
                        new Customer
                        {
                            Id = 5,
                            Name = "Cliente",
                            IdentityUserId = "identity123"
                        }
                    });

                var result = await _service.GetCustomerByIdentityUserIdAsync("identity123");

                result.Should().NotBeNull();
                result!.Id.Should().Be(5);
            }
        }
    }
