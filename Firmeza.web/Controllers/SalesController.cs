using Firmeza.Application.DTOs.Sales;
using Firmeza.Application.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.web.Controllers;

[Authorize(Roles = "Administrator")]
public class SalesController : Controller
{
    private readonly IProductService _productService;
    private readonly ISaleService _saleService;
    private readonly ICustomerService _customerService;

    public SalesController(ISaleService saleService, IProductService productService, ICustomerService customerService)
    {
        _saleService = saleService;
        _customerService = customerService;
        _productService = productService;
    }

    public async Task<IActionResult> Index()
    {
        ViewData["Title"] = "Sales Management";
        var sales = await _saleService.GetAllSalesAsync();
        return View(sales);
    }

    public async Task<IActionResult> Create()
    {
        ViewData["Title"] = "Create Sales";
        ViewBag.Customers = await _customerService.GetAllCustomersAsync();
        ViewBag.Products= await _productService.GetAllProductsAsync();
        
        return View(new SaleCreateViewModel());
    }

    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(SaleCreateViewModel model)
    {
        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Register sale";
            ViewBag.Customers = await _customerService.GetAllCustomersAsync();
            ViewBag.Products = await _productService.GetAllProductsAsync();
            return View(model);
        }
        
        await _saleService.CreateSaleAsync(model);
        
        TempData["Message"] = "Sale created successfully";
        return RedirectToAction(nameof(Index));
    }

    [HttpGet]
    public async Task<IActionResult> Edit(int id)
    {
        var sale = await _saleService.GetSaleForEditAsync(id);
        if (sale == null) return NotFound();

        var model = new SaleEditViewModel
        {
            Id = sale.Id,
            CustomerId = sale.CustomerId,
            SaleDetails = sale.SaleDetails.Select(d => new SaleDetailEditViewModel
            {
                Id = d.Id,
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                AppliedUnitPrice = d.AppliedUnitPrice
            }).ToList(),
            Subtotal = sale.Subtotal,
            Iva = sale.Iva,
            Total = sale.Total
        };
        
        ViewBag.Customers = await _customerService.GetAllCustomersAsync();
        ViewBag.Products = await _productService.GetAllProductsAsync();
        
        return View(model);
    }


    [HttpPost]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, SaleEditViewModel model)
    {
        if (id != model.Id) return NotFound();

        if (!ModelState.IsValid)
        {
            ViewData["Title"] = "Edit Sale";
            ViewBag.Customers = await _customerService.GetAllCustomersAsync();
            ViewBag.Products = await _productService.GetAllProductsAsync();
            return View(model);
        }

        var success = await _saleService.UpdateSaleAsync(model);

        if (!success)
        {
            ModelState.AddModelError("", "Could not update the sale. It may have been deleted or changed by another user.");
            return View(model);
        }

        TempData["SuccessMessage"] = "Sale updated successfully!";
        return RedirectToAction(nameof(Index));
    }

    [HttpPost, ActionName("Delete")]
    [ValidateAntiForgeryToken]
    public async Task<IActionResult> Delete(int id)
    {
        var success = await _saleService.DeleteSaleAsync(id);

        if (success)
            TempData["successMessage"] = "Sale deleted  successfully!";
        else
            TempData["ErrorMessage"] = "Could not delete the sale. it may not exist or has related data.";
        
        return RedirectToAction(nameof(Index));
    }
    
    [HttpGet]
    public IActionResult DownloadReceipt(string fileName)
    {
        if (string.IsNullOrEmpty(fileName))
            return NotFound();
        
        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot", "Receipt", fileName);
        if (!System.IO.File.Exists(path))
            return NotFound();
        
        var fileBytes = System.IO.File.ReadAllBytes(path);
        return File(fileBytes,"application/pdf", fileName);
        
    }
    
    
}