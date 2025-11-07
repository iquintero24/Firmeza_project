using Firmeza.Application.DTOs.Customers;

namespace Firmeza.Application.Interfaces;

public interface ICustomerService
{
    // return the list of viewModels for the view Index
    Task<IEnumerable<CustomerIndexViewModel>> GetAllCustomersAsync();

    Task<CustomerEditViewModel> GetCustomerForEditAsync(int id);
    
    Task<CustomerIndexViewModel> CreateCustomerAsync(CustomerCreateViewModel model);
    
    Task<bool> UpdateCustomerAsync(CustomerEditViewModel model);
    
    Task<bool> DeleteCustomerAsync(int id);
    
    Task<bool> IsDocumentOrEmailDuplicateAsync(int id, string document, string email);
}