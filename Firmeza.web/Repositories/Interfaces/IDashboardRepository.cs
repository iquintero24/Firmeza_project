namespace Firmeza.web.Repositories.Interfaces;

public interface IDashboardRepository
{
    // Method to get the total count for products, Customer and Sales.
    Task<(int totalProducts, int totalCustomer, int totalSales)> GetDashboardMetricsAsync();
}