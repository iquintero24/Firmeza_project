using Firmeza.web.Models.ViewModels.Products;
using Firmeza.web.Services.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.web.Controllers;

// restrict the access only users with rol of admin
[Authorize(Roles = "Administrator")]
public class ProductsController: Controller
{
    private readonly IProductService _productService;
    
    // inject of dependency's
    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }
    
    /// <summary>
    /// Index for product (List of products)
    /// </summary>
    /// <returns></returns>
    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Products Management";
        
        // call the service for get the list of product
        var products = await _productService.GetAllProductsAsync();
        
        // return the list of viewModels the view
        return View(products);
    }
    
    /// <summary>
    /// show of form for create
    /// </summary>
    /// <returns></returns>
    public IActionResult Create()
    {
        ViewData["Title"] = "Create Product";
        return View();
    }
    
    /// <summary>
    /// create the product.
    /// </summary>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(ProductCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Register New Product";
            return View(model);
        }
        
        // call the service 
        await _productService.CreateProductAsync(model);
        
        // add message of success
        TempData["SuccessMessage"] = $"Product {model.Name} created successfully!";
        
        return RedirectToAction(nameof(Index));
    }
    
    /// <summary>
    /// Get the view for edit product
    /// </summary>
    /// <param name="id"></param>
    /// <returns></returns>
    public async Task<IActionResult> Edit(int id)
    {
        var model = await _productService.GetProductForEditAsync(id);
        
        if (model == null) return NotFound();
        
        ViewData["Title"] = "Edit Product";
        return View(model);
    }
    
    /// <summary>
    /// method post of edit 
    /// </summary>
    /// <param name="id"></param>
    /// <param name="model"></param>
    /// <returns></returns>
    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, ProductEditViewModel model)
    {
        // valid that id of the url coincide
        if (id != model.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = $"Edit Product: {model.Name}";
            return View(model);
        }
        
        // call of teh servicio 
        var success = await _productService.UpdateProductAsync(model);

        if (!success)
        {
            // if failed by error of concurrency or no exist 
            ModelState.AddModelError("", "Could not update the product. It may been deleted bt another user.");
            return View(model);
        }

        TempData["SuccessMessage"] = $"Product {model.Name} update Successfully!";
        return RedirectToAction(nameof(Index));
    }
    
    [HttpPost,ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var success = await _productService.DeleteProductAsync(id);

        if (success)
        {
            TempData["SuccessMessage"] = "Product delete successfully";
        }
        else
        {
            TempData["ErrorMessage"] = "Could not delete the product. It may not exist or has associated records";
        }
        
        return RedirectToAction(nameof(Index));
    }
    
}