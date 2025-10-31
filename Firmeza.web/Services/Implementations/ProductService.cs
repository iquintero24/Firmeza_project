using Firmeza.web.Data.Entities;
using Firmeza.web.Models.ViewModels.Products;
using Firmeza.web.Repositories.Interfaces;
using Firmeza.web.Services.Interfaces;

namespace Firmeza.web.Services.Implementations;

public class ProductService:IProductService
{

    private readonly IProductRepository _productRepository;

    public ProductService(IProductRepository productRepository)
    {
        _productRepository = productRepository;
    }
    
    public async Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync()
    {
        var products = await _productRepository.GetAllAsync();
        
        // maps manual entity -> ProductIndexViewModel
        // use select for maps the list 
        return products.Select(p => new ProductIndexViewModel
        {
            Id = p.Id,
            Name = p.Name,
            UnitPrice = p.UnitPrice,
            Stock = p.Stock
        }).ToList();
    }

    public async Task<ProductEditViewModel> GetProductForEditAsync(int id)
    {
        var product = await _productRepository.GetByIdAsync(id);
        if (product == null) return null;
        
        // maps Entities -> ProductEditViewModel
        return new ProductEditViewModel
        {
            Id = product.Id,
            Name = product.Name,
            Description = product.Description,
            UnitPrice = product.UnitPrice,
            Stock = product.Stock
        };
    }

    public async Task<ProductIndexViewModel> CreateProductAsync(ProductCreateViewModel model)
    {
       // maps 
       var productEntity = new Product
       {
           Name = model.Name,
           Description = model.Description,
           UnitPrice = model.UnitPrice,
           Stock = model.Stock
       };
       
       // pass teh entity the repository for save 
       var savedProduct = await _productRepository.AddAsync(productEntity);

       return new ProductIndexViewModel
       {
           Id = savedProduct.Id,
           Name = savedProduct.Name,
           UnitPrice = savedProduct.UnitPrice,
           Stock = savedProduct.Stock
       };
    }

    public async Task<bool> UpdateProductAsync(ProductEditViewModel model)
    {
        var existingProduct = await _productRepository.GetByIdAsync(model.Id);
        if (existingProduct == null) return false;

        // Maps ProductEditViewModel -> Entity exist
        existingProduct.Name = model.Name;
        existingProduct.Description = model.Description;
        existingProduct.UnitPrice = model.UnitPrice;
        existingProduct.Stock = model.Stock;

        
        return await _productRepository.UpdateAsync(existingProduct);
    }

    public async Task<bool> DeleteProductAsync(int id)
    {
        return await _productRepository.DeleteAsync(id);
    }
}