using Firmeza.Application.DTOs.Sales;

namespace Firmeza.Application.Interfaces;

public interface ISaleService
{
    Task<IEnumerable<SaleIndexViewModel>> GetAllSalesAsync();
    Task<SaleEditViewModel?> GetSaleForEditAsync(int id);
    Task<SaleIndexViewModel> CreateSaleAsync(SaleCreateViewModel model);
    Task<bool> UpdateSaleAsync(SaleEditViewModel model);
    Task<bool> DeleteSaleAsync(int id);
}