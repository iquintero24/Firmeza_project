using Firmeza.web.Data.Entities;
using Firmeza.web.Models.ViewModels.Products;

namespace Firmeza.web.Services.Interfaces;

public interface IProductService
{
    // return a list in viewModels
    Task<IEnumerable<ProductIndexViewModel>> GetAllProductsAsync();
    
    //Edit (Get): search and return a ViewModel of edit
    Task<ProductEditViewModel> GetProductForEditAsync(int id);
    
    // Create (Post): 
    Task<ProductIndexViewModel> CreateProductAsync(ProductCreateViewModel model);
    
    //editPost(POST) 
    Task<bool> UpdateProductAsync(ProductEditViewModel model);
    
    //Delete
    Task<bool> DeleteProductAsync(int id);

}