using Firmeza.Domain.Interfaces;
using Firmeza.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore;

namespace Firmeza.Infrastructure.Persistence.Repositories;

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