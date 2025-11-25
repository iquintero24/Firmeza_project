using Firmeza.Application.DTOs.Customers;
using Firmeza.Application.Interfaces;
using Firmeza.Domain.Entities;
using Firmeza.Domain.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.Application.Implemetations;

public class CustomerService : ICustomerService
{
    private readonly ICustomerRepository _customerRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerService(ICustomerRepository customerRepository, UserManager<ApplicationUser> userManager)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
    }

    public async Task<IEnumerable<CustomerIndexViewModel>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();

        return customers.Select(c => new CustomerIndexViewModel
        {
            Id = c.Id,
            Name = c.Name,
            Document = c.Document,
            Email = c.Email,
            Phone = c.Phone,
        }).ToList();
    }

    public async Task<CustomerEditViewModel> GetCustomerForEditAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return null;

        return new CustomerEditViewModel
        {
            Id = customer.Id,
            Name = customer.Name,
            Document = customer.Document,
            Email = customer.Email,
            Phone = customer.Phone,
        };
    }

    // CREATE
    public async Task<CustomerIndexViewModel> CreateCustomerAsync(CustomerCreateViewModel model)
    {
        if (await IsDocumentOrEmailDuplicateAsync(0, model.Document, model.Email))
            throw new InvalidOperationException("A customer with this Document or Email already exists.");

        var user = new ApplicationUser
        {
            UserName = model.Email,
            Email = model.Email,
            EmailConfirmed = true
        };

        var result = await _userManager.CreateAsync(user, model.Password);

        if (!result.Succeeded)
        {
            var errors = string.Join(", ", result.Errors.Select(e => e.Description));
            throw new InvalidOperationException($"Failed to create user account: {errors}");
        }

        var customerEntity = new Customer
        {
            Name = model.Name,
            Document = model.Document,
            Email = model.Email,
            Phone = model.Phone,
            IdentityUserId = user.Id,
            Sales = new List<Sale>()
        };

        var savedCustomer = await _customerRepository.AddAsync(customerEntity);

        return new CustomerIndexViewModel
        {
            Id = savedCustomer.Id,
            Name = savedCustomer.Name,
            Document = savedCustomer.Document,
            Email = savedCustomer.Email,
            Phone = savedCustomer.Phone
        };
    }

    // UPDATE
    public async Task<bool> UpdateCustomerAsync(CustomerEditViewModel model)
    {
        if (await IsDocumentOrEmailDuplicateAsync(model.Id, model.Document, model.Email))
            throw new InvalidOperationException("Another customer already uses this Document or Email.");

        var existingCustomer = await _customerRepository.GetByIdAsync(model.Id);
        if (existingCustomer == null) return false;

        existingCustomer.Name = model.Name;
        existingCustomer.Document = model.Document;
        existingCustomer.Email = model.Email;
        existingCustomer.Phone = model.Phone;

        return await _customerRepository.UpdateAsync(existingCustomer);
    }

    // DELETE
    public async Task<bool> DeleteCustomerAsync(int id)
    {
        var customer = await _customerRepository.GetByIdAsync(id);
        if (customer == null) return false;

        if (customer.Sales != null && customer.Sales.Any())
            throw new InvalidOperationException("Cannot delete customer because they have associated sales.");

        var success = await _customerRepository.DeleteAsync(id);

        if (success && customer.IdentityUserId != null)
        {
            var user = await _userManager.FindByIdAsync(customer.IdentityUserId);
            if (user != null)
                await _userManager.DeleteAsync(user);
        }

        return success;
    }

    // DUPLICADO
    public async Task<bool> IsDocumentOrEmailDuplicateAsync(int id, string document, string email)
    {
        var allCustomers = await _customerRepository.GetAllAsync();

        return allCustomers.Any(c =>
            (c.Document.Equals(document, StringComparison.OrdinalIgnoreCase) ||
             c.Email.Equals(email, StringComparison.OrdinalIgnoreCase))
            && c.Id != id);
    }

    // ===========================================================
    // 🔵 NUEVO MÉTODO: Obtener Customer por IdentityUserId
    // ===========================================================

    public async Task<CustomerIndexViewModel?> GetCustomerByIdentityUserIdAsync(string identityUserId)
    {
        var customers = await _customerRepository.GetAllAsync();

        var customer = customers.FirstOrDefault(c => c.IdentityUserId == identityUserId);

        if (customer == null) return null;

        return new CustomerIndexViewModel
        {
            Id = customer.Id,
            Name = customer.Name,
            Document = customer.Document,
            Email = customer.Email,
            Phone = customer.Phone,
        };
    }
}
