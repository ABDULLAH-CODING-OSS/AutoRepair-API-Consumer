using ERP_Consumer.Services.Interfaces;
using ERP_Consumer.ViewModels;
using Microsoft.AspNetCore.Mvc;

namespace ERP_Consumer.Controllers;

public class DashboardController : Controller
{
    private readonly ICustomerApiService _customers;
    private readonly IVehicleApiService _vehicles;
    private readonly ICategoryApiService _categories;
    private readonly IServiceApiService _services;
    private readonly IPartApiService _parts;
    private readonly ILogger<DashboardController> _logger;

    public DashboardController(
        ICustomerApiService customers,
        IVehicleApiService vehicles,
        ICategoryApiService categories,
        IServiceApiService services,
        IPartApiService parts,
        ILogger<DashboardController> logger)
    {
        _customers = customers;
        _vehicles = vehicles;
        _categories = categories;
        _services = services;
        _parts = parts;
        _logger = logger;
    }

    public async Task<IActionResult> Index()
    {
        var customerTask = _customers.GetAllAsync();
        var vehicleTask = _vehicles.GetAllAsync();
        var categoryTask = _categories.GetAllAsync();
        var serviceTask = _services.GetAllAsync();
        var partTask = _parts.GetAllAsync();

        await Task.WhenAll(customerTask, vehicleTask, categoryTask, serviceTask, partTask);

        var vm = new DashboardViewModel
        {
            TotalCustomers = customerTask.Result.Success ? customerTask.Result.Data?.Count() ?? 0 : 0,
            TotalVehicles = vehicleTask.Result.Success ? vehicleTask.Result.Data?.Count() ?? 0 : 0,
            TotalCategories = categoryTask.Result.Success ? categoryTask.Result.Data?.Count() ?? 0 : 0,
            TotalServices = serviceTask.Result.Success ? serviceTask.Result.Data?.Count() ?? 0 : 0,
            TotalParts = partTask.Result.Success ? partTask.Result.Data?.Count() ?? 0 : 0,
            ApiOnline = customerTask.Result.StatusCode != 503
        };

        return View(vm);
    }
}
