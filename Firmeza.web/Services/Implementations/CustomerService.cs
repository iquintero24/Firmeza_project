using Firmeza.Domain.Entities;
using Firmeza.Domain.Interfaces;
using Firmeza.web.Models.ViewModels.Customers;
using Firmeza.web.Services.Interfaces;
using Microsoft.AspNetCore.Identity;

namespace Firmeza.web.Services.Implementations;

public class CustomerService: ICustomerService
{
    
    private readonly ICustomerRepository _customerRepository;
    private readonly UserManager<ApplicationUser> _userManager;

    public CustomerService(ICustomerRepository customerRepository , UserManager<ApplicationUser> userManager)
    {
        _customerRepository = customerRepository;
        _userManager = userManager;
    }
    
   public async Task<IEnumerable<CustomerIndexViewModel>> GetAllCustomersAsync()
        {
            var customers = await _customerRepository.GetAllAsync();
            
            // Mapeo Manual: Entidad -> CustomerIndexViewModel
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
            
            // Mapeo Manual: Entidad -> CustomerEditViewModel
            return new CustomerEditViewModel
            {
                Id = customer.Id,
                Name = customer.Name,
                Document = customer.Document,
                Email = customer.Email,
                Phone = customer.Phone,
            };
        }
        
        // -----------------------------------------------------------------
        // CREATE (Integrando Identity)
        // -----------------------------------------------------------------

        public async Task<CustomerIndexViewModel> CreateCustomerAsync(CustomerCreateViewModel model)
        {
            // 1. Lógica de Validación de Negocio: Documento o Email duplicado
            if (await IsDocumentOrEmailDuplicateAsync(0, model.Document, model.Email))
            {
                // Usamos InvalidOperationException para un error de negocio que el Controller debe capturar
                throw new InvalidOperationException("A customer with this Document or Email already exists in the business data.");
            }

            // 2. CREACIÓN DEL USUARIO IDENTITY (Email como UserName)
            var user = new ApplicationUser 
            { 
                UserName = model.Email, 
                Email = model.Email,
                EmailConfirmed = true // Permite login inmediato
            };
            
            var result = await _userManager.CreateAsync(user, model.Password);

            if (!result.Succeeded)
            {
                // Si Identity falla (ej: contraseña débil, email duplicado en AspNetUsers)
                var errors = string.Join(", ", result.Errors.Select(e => e.Description));
                // Lanza un error que el Controller debe mostrar
                throw new InvalidOperationException($"Failed to create user account: {errors}"); 
            }
            
            // 3. ASIGNAR ROL (Si tienes un rol 'Customer' definido)
            // await _userManager.AddToRoleAsync(user, "Customer"); 

            // 4. CREACIÓN DE LA ENTIDAD DE NEGOCIO (Customer)
            var customerEntity = new Customer
            {
                Name = model.Name,
                Document = model.Document,
                Email = model.Email,
                Phone = model.Phone,
                IdentityUserId = user.Id, // VINCULACIÓN CRUCIAL con el nuevo usuario
                Sales = new List<Sale>()
            };

            var savedCustomer = await _customerRepository.AddAsync(customerEntity);
            
            // Mapeo Manual de vuelta: Entidad -> CustomerIndexViewModel
            return new CustomerIndexViewModel
            {
                Id = savedCustomer.Id,
                Name = savedCustomer.Name,
                Document = savedCustomer.Document,
                Email = savedCustomer.Email,
                Phone = savedCustomer.Phone
            };
        }
        
        // -----------------------------------------------------------------
        // UPDATE
        // -----------------------------------------------------------------

        public async Task<bool> UpdateCustomerAsync(CustomerEditViewModel model)
        {
            // 1. Lógica de Validación de Negocio (excluyendo el propio registro)
            if (await IsDocumentOrEmailDuplicateAsync(model.Id, model.Document, model.Email))
            {
                throw new InvalidOperationException("Another customer already uses this Document or Email.");
            }
            
            var existingCustomer = await _customerRepository.GetByIdAsync(model.Id);
            if (existingCustomer == null) return false;

            // 2. Mapeo Manual: CustomerEditViewModel -> Entidad existente
            existingCustomer.Name = model.Name;
            existingCustomer.Document = model.Document;
            existingCustomer.Email = model.Email;
            existingCustomer.Phone = model.Phone;

            // Nota: Aquí no se actualiza la contraseña, ya que eso es una operación separada en Identity.
            // Si el email cambia, deberías actualizar el Email y UserName en IdentityUser.
            
            return await _customerRepository.UpdateAsync(existingCustomer);
        }
        
        // -----------------------------------------------------------------
        // DELETE
        // -----------------------------------------------------------------

        public async Task<bool> DeleteCustomerAsync(int id)
        {
            var customer = await _customerRepository.GetByIdAsync(id);
            if (customer == null) return false;

            // 1. Integridad Referencial: Verificar si tiene ventas asociadas
            if (customer.Sales != null && customer.Sales.Any())
            {
                 throw new InvalidOperationException("Cannot delete customer because they have associated sales records.");
            }
            
            // 2. Eliminación de la entidad de negocio
            var success = await _customerRepository.DeleteAsync(id);

            if (success && customer.IdentityUserId != null)
            {
                // 3. Eliminación del usuario Identity asociado
                var user = await _userManager.FindByIdAsync(customer.IdentityUserId);
                if (user != null)
                {
                    await _userManager.DeleteAsync(user);
                }
            }

            return success;
        }

        // -----------------------------------------------------------------
        // VALIDACIÓN DE UNICIDAD
        // -----------------------------------------------------------------

        public async Task<bool> IsDocumentOrEmailDuplicateAsync(int id, string document, string email)
        {
            var AllCustomers = await _customerRepository.GetAllAsync();

            return AllCustomers.Any(c => 
                (c.Document.Equals(document, StringComparison.OrdinalIgnoreCase) || 
                 c.Email.Equals(email, StringComparison.OrdinalIgnoreCase)) && 
                c.Id != id);
        }
    
    
}