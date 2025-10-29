using Firmeza.web.Data;
using Firmeza.web.Repositories.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.web.Repositories.Implementations;

public class DashboardRepository: IDashboardRepository
{
    private readonly ApplicationDbContext _dbContext;

    public DashboardRepository(ApplicationDbContext dbContext)
    {
        _dbContext = dbContext;
    }
    
    public async Task<(int totalProducts, int totalCustomer, int totalSales)> GetDashboardMetricsAsync()
    {
        // fetch metrics using asynchronous counting 
        var totalProducts = await _dbContext.Products.CountAsync();
        var totalCustomers = await _dbContext.Customers.CountAsync();
        var totalSales = await _dbContext.Sales.CountAsync();
        return (totalProducts, totalCustomers, totalSales);
    }
}