using Firmeza.Application.DTOs.Sales;
using Firmeza.Application.Interfaces;
using Firmeza.Domain.Entities;
using Firmeza.Domain.Interfaces;
using Infrastructure.Utils;
using Microsoft.AspNetCore.Hosting;

namespace Firmeza.Application.Implemetations;

public class SaleService : ISaleService
{
    private readonly ISaleRepository _saleRepository;
    private readonly ICustomerRepository _customerRepository;
    private readonly IProductRepository _productRepository;
    private readonly IEmailService _emailService;
    private readonly IWebHostEnvironment _env;

    public SaleService(ISaleRepository saleRepository, ICustomerRepository customerRepository,
        IProductRepository productRepository, IEmailService emailService, IWebHostEnvironment env)
    {
        _saleRepository = saleRepository;
        _customerRepository = customerRepository;
        _productRepository = productRepository;
        _emailService = emailService;
        _env = env; 
        
    }

    // =========================================
    // GET ALL SALES
    // =========================================
    public async Task<IEnumerable<SaleIndexViewModel>> GetAllSalesAsync()
    {
        var sales = await _saleRepository.GetAllAsync();

        return sales.Select(s => new SaleIndexViewModel
        {
            Id = s.Id,
            CustomerName = s.Customer.Name,
            SaleDate = s.SaleDate,
            ReceiptNumber = s.ReceiptNumber,
            Total = s.Total
        }).ToList();
    }

    // =========================================
    // GET SALE BY ID (for edit)
    // =========================================
    public async Task<SaleEditViewModel?> GetSaleForEditAsync(int id)
    {
        var sale = await _saleRepository.GetByIdAsync(id);
        if (sale == null) return null;

        return new SaleEditViewModel
        {
            Id = sale.Id,
            CustomerId = sale.CustomerId,
            SaleDate = sale.SaleDate,
            ReceiptNumber = sale.ReceiptNumber,
            Subtotal = sale.Subtotal,
            Iva = sale.Iva,
            Total = sale.Total,
            SaleDetails = sale.SaleDetails.Select(d => new SaleDetailEditViewModel
            {
                Id = d.Id,
                ProductId = d.ProductId,
                Quantity = d.Quantity,
                AppliedUnitPrice = d.AppliedUnitPrice
            }).ToList()
        };
    }

    // =========================================
    // CREATE SALE (ya lo tenías)
    // =========================================
   public async Task<SaleIndexViewModel> CreateSaleAsync(SaleCreateViewModel model)
{
    var customer = await _customerRepository.GetByIdAsync(model.CustomerId);
    if (customer == null)
        throw new InvalidOperationException($"Customer with id {model.CustomerId} does not exist");

    var sale = new Sale
    {
        CustomerId = model.CustomerId,
        SaleDate = DateTime.UtcNow,
        ReceiptNumber = Guid.NewGuid().ToString().Substring(0, 8).ToUpper(),
        Subtotal = model.Subtotal,
        Iva = model.Iva,
        Total = model.Total,
        Customer = customer,
        SaleDetails = new List<SaleDetail>()
    };

    foreach (var item in model.SaleDetails)
    {
        var product = await _productRepository.GetByIdAsync(item.ProductId);
        if (product == null)
            throw new InvalidOperationException($"Product with id {item.ProductId} does not exist");

        if (product.Stock < item.Quantity)
            throw new InvalidOperationException($"Product with id {item.ProductId} does not have enough stock");

        product.Stock -= item.Quantity;
        await _productRepository.UpdateAsync(product);

        sale.SaleDetails.Add(new SaleDetail
        {
            ProductId = item.ProductId,
            Quantity = item.Quantity,
            AppliedUnitPrice = item.AppliedUnitPrice,
            Product = product
        });
    }

    var savedSale = await _saleRepository.AddAsync(sale);

    // 1️⃣ Generar PDF
    var pdfRelativePath = PdfGenerator.GenerateSaleReceipt(savedSale);

    // Convertimos a ruta absoluta en disco
    var pdfFullPath = Path.Combine(_env.WebRootPath, "Receipt", Path.GetFileName(pdfRelativePath));

    // 2️⃣ Enviar correo con el PDF adjunto
    if (!string.IsNullOrWhiteSpace(customer.Email))
    {
        await _emailService.SendEmailWithPdf(
            toEmail: customer.Email,
            subject: "Your Purchase Receipt",
            body: $"<p>Hi {customer.Name},</p><p>Thanks for your purchase! Your receipt is attached.</p>",
            pdfFullPath: pdfFullPath
        );
    }

    // 3️⃣ Retornar respuesta al frontend
    return new SaleIndexViewModel
    {
        Id = savedSale.Id,
        CustomerName = customer.Name,
        SaleDate = savedSale.SaleDate,
        ReceiptNumber = savedSale.ReceiptNumber,
        Total = savedSale.Total,
        PdfUrl = pdfRelativePath
    };
}


    // =========================================
    // UPDATE SALE
    // =========================================
    public async Task<bool> UpdateSaleAsync(SaleEditViewModel model)
    {
        var existing = await _saleRepository.GetByIdAsync(model.Id);
        if (existing == null) return false;

        existing.SaleDetails ??= new List<SaleDetail>();
        
        foreach (var oldDetail in existing.SaleDetails.ToList())
        {
            var product = await _productRepository.GetByIdAsync(oldDetail.ProductId);
            if (product != null)
            {
                product.Stock += oldDetail.Quantity;
                await _productRepository.UpdateAsync(product);
            }
        }
        
        existing.SaleDetails.Clear();
        
        
        foreach (var detail in model.SaleDetails)
        {
            var product = await _productRepository.GetByIdAsync(detail.ProductId);
            if (product == null)
                throw new InvalidOperationException($"Product {detail.ProductId} not found");

            if (product.Stock < detail.Quantity)
                throw new InvalidOperationException($"Not enough stock for product {product.Name}");

            product.Stock -= detail.Quantity;
            await _productRepository.UpdateAsync(product);

            existing.SaleDetails.Add(new SaleDetail
            {
                ProductId = detail.ProductId,
                Quantity = detail.Quantity,
                AppliedUnitPrice = detail.AppliedUnitPrice,
                Product = product
            });
        }
        
        existing.CustomerId = model.CustomerId;
        existing.SaleDate = model.SaleDate;
        existing.Subtotal = model.Subtotal;
        existing.Iva = model.Iva;
        existing.Total = model.Total;

        return await _saleRepository.UpdateAsync(existing);
    }

    // =========================================
    // DELETE SALE
    // =========================================
    public async Task<bool> DeleteSaleAsync(int id)
    {
        var existing = await _saleRepository.GetByIdAsync(id);
        if (existing == null) return false;

        foreach (var detail in existing.SaleDetails)
        {
            var product = await _productRepository.GetByIdAsync(detail.ProductId);
            if (product != null)
            {
                product.Stock += detail.Quantity;
                await _productRepository.UpdateAsync(product);
            }
        }

        return await _saleRepository.DeleteAsync(id);
    }
    
    // =========================================
// GET SALES BY CUSTOMER (Historial de compras)
// =========================================
    public async Task<IEnumerable<SaleIndexViewModel>> GetSalesByCustomerAsync(int customerId)
    {
        var sales = await _saleRepository.GetSalesByCustomerAsync(customerId);

        return sales.Select(s => new SaleIndexViewModel
        {
            Id = s.Id,
            CustomerName = s.Customer.Name,
            SaleDate = s.SaleDate,
            ReceiptNumber = s.ReceiptNumber,
            Total = s.Total,
            PdfUrl = null // o simplemente NO incluirlo
        }).ToList();

    }
    
    

}
