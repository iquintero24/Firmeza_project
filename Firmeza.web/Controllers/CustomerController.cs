using Firmeza.web.Models.ViewModels.Customers;
using Firmeza.web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.web.Controllers;

public class CustomerController
{
    // Restringe el acceso solo a usuarios con el rol "Administrador"
    [Authorize(Roles = "Administrador")] 
    public class CustomersController : Controller
    {
        private readonly ICustomerService _customerService;

        // Inyección de Dependencias
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        // -----------------------------------------------------------------
        // 1. INDEX (READ - Listado de Clientes)
        // -----------------------------------------------------------------
        public async Task<IActionResult> Index()
        {
            ViewData["Title"] = "Customers Management";
            var customers = await _customerService.GetAllCustomersAsync();
            return View(customers);
        }

        // -----------------------------------------------------------------
        // 2. CREATE (GET - Mostrar Formulario)
        // -----------------------------------------------------------------
        public IActionResult Create()
        {
            ViewData["Title"] = "Register New Customer";
            return View();
        }

        // -----------------------------------------------------------------
        // 2. CREATE (POST - Procesar Formulario con Manejo de Errores)
        // -----------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(CustomerCreateViewModel model)
        {
            if (!ModelState.IsValid)
            {
                ViewData["Title"] = "Register New Customer";
                // Si la validación de ViewModel falla (ej: password mismatch), regresa
                return View(model); 
            }

            try
            {
                // Llama al servicio para crear el cliente y el usuario Identity
                await _customerService.CreateCustomerAsync(model);
                
                TempData["SuccessMessage"] = $"Customer '{model.Name}' and associated user account created successfully!";
                return RedirectToAction(nameof(Index));
            }
            // Captura las excepciones de lógica de negocio lanzadas por el servicio
            catch (InvalidOperationException ex) 
            {
                // Muestra el mensaje de error de unicidad o de Identity (ej: contraseña débil)
                ModelState.AddModelError("", ex.Message);
                ViewData["Title"] = "Register New Customer";
                return View(model);
            }
        }

        // -----------------------------------------------------------------
        // 3. EDIT (GET - Mostrar Formulario con Datos)
        // -----------------------------------------------------------------
        public async Task<IActionResult> Edit(int id)
        {
            var model = await _customerService.GetCustomerForEditAsync(id);
            if (model == null) return NotFound();

            ViewData["Title"] = $"Edit Customer: {model.Name}";
            return View(model);
        }

        // -----------------------------------------------------------------
        // 3. EDIT (POST - Procesar Actualización con Manejo de Errores)
        // -----------------------------------------------------------------
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, CustomerEditViewModel model)
        {
            if (id != model.Id) return NotFound(); 

            if (!ModelState.IsValid)
            {
                ViewData["Title"] = $"Edit Customer: {model.Name}";
                return View(model);
            }

            try
            {
                var success = await _customerService.UpdateCustomerAsync(model);

                if (!success)
                {
                    ModelState.AddModelError("", "Could not update the customer. The customer might have been deleted.");
                    return View(model);
                }
                
                TempData["SuccessMessage"] = $"Customer '{model.Name}' updated successfully!";
                return RedirectToAction(nameof(Index));
            }
            catch (InvalidOperationException ex)
            {
                // Muestra el error de unicidad (documento/email duplicado)
                ModelState.AddModelError("", ex.Message);
                ViewData["Title"] = $"Edit Customer: {model.Name}";
                return View(model);
            }
        }

        // -----------------------------------------------------------------
        // 4. DELETE (POST - Eliminar con Manejo de Errores)
        // -----------------------------------------------------------------
        [HttpPost, ActionName("Delete")] 
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            try
            {
                var success = await _customerService.DeleteCustomerAsync(id);
                
                if (success)
                {
                     TempData["SuccessMessage"] = "Customer and associated user deleted successfully.";
                }
                else
                {
                     TempData["ErrorMessage"] = "Could not delete the customer. It may not exist.";
                }
            }
            catch (InvalidOperationException ex)
            {
                 // Captura el error de integridad referencial (tiene ventas)
                 TempData["ErrorMessage"] = ex.Message; 
            }
            
            return RedirectToAction(nameof(Index));
        }
    }
}