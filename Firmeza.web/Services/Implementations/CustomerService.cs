using Firmeza.web.Models.ViewModels.Customers;
using Firmeza.web.Repositories.Interfaces;
using Firmeza.web.Services.Interfaces;

namespace Firmeza.web.Services.Implementations;

public class CustomerService: ICustomerService
{
    
    private readonly ICustomerRepository _customerRepository;

    public CustomerService(ICustomerRepository customerRepository)
    {
        _customerRepository = customerRepository;
    }
    
    public async Task<IEnumerable<CustomerIndexViewModel>> GetAllCustomersAsync()
    {
        var customers = await _customerRepository.GetAllAsync();
        
        // map the object in the viewModel
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
        
        // maps entitie -> CustomerEditViewModel
        return new CustomerEditViewModel
        {
            Id = customer.Id,
            Name = customer.Name,
            Document = customer.Document,
            Email = customer.Email,
            Phone = customer.Phone,
        };
    }

    public async Task<CustomerIndexViewModel> CreateCustomerAsync(CustomerCreateViewModel model)
    {
        // logic valid business: Document o email duplicated
        if (await IsDocumentOrEmailDuplicateAsync(0, model.Document, model.Email))
        {
            throw new Exception("A Customer with this Document or Email already exists.");
        }

        return new CustomerIndexViewModel();
    }

    public Task<bool> UpdateCustomerAsync(CustomerEditViewModel model)
    {
        throw new NotImplementedException();
    }

    public Task<bool> DeleteCustomerAsync(int id)
    {
        throw new NotImplementedException();
    }

    public async Task<bool> IsDocumentOrEmailDuplicateAsync(int id, string document, string email)
    {
        var AllCustomers = await _customerRepository.GetAllAsync();

        return AllCustomers.Any(c => (c.Document.Equals(document, StringComparison.OrdinalIgnoreCase) || c.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) && c.Id != id);
    }
    
    
}