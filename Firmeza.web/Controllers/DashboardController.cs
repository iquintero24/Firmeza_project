using Firmeza.Domain.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Firmeza.web.Controllers;

// the controller is protected
[Authorize(Roles = "Administrator")]
public class DashboardController: Controller
{
    // Inject the repository Interface instead of the dbContext
    private readonly IDashboardRepository _dashboardRepository;

    public DashboardController(IDashboardRepository dashboardRepository)
    {
        _dashboardRepository = dashboardRepository;
    }

    public async Task<IActionResult> Index()
    {
        // fetch metric using the repositories layer 
        var metrics = await _dashboardRepository.GetDashboardMetricsAsync();
        
        // Pass metrics to the view using viewData
        ViewData["TotalProducts"] = metrics.totalProducts;
        ViewData["TotalCustomers"] = metrics.totalCustomer;
        ViewData["TotalSales"] = metrics.totalSales;

        ViewData["Title"] = "Dashboard Overview";
        return View();
        
    }
}