using ERP_Consumer.DTOs.Vehicles;
using ERP_Consumer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_Consumer.Controllers;

public class VehiclesController : Controller
{
    private readonly IVehicleApiService _service;
    private readonly ICustomerApiService _customerService;
    private readonly ILogger<VehiclesController> _logger;

    public VehiclesController(
        IVehicleApiService service,
        ICustomerApiService customerService,
        ILogger<VehiclesController> logger)
    {
        _service = service;
        _customerService = customerService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var result = await _service.GetAllAsync();
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return View(Enumerable.Empty<VehicleDto>());
        }

        var data = result.Data!;
        if (!string.IsNullOrWhiteSpace(search))
        {
            data = data.Where(v =>
                (v.Make ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (v.Model ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (v.LicensePlate ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase) ||
                (v.CustomerName ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase));
        }

        ViewBag.Search = search;
        return View(data);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return RedirectToAction(nameof(Index)); }
        return View(result.Data);
    }

    public async Task<IActionResult> Create()
    {
        await PopulateCustomersDropdown();
        return View(new CreateVehicleDto { ManufacturingYear = DateTime.Now.Year });
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateVehicleDto dto)
    {
        if (!ModelState.IsValid) { await PopulateCustomersDropdown(dto.CustomerId); return View(dto); }

        var result = await _service.CreateAsync(dto);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Failed to create vehicle.");
            await PopulateCustomersDropdown(dto.CustomerId);
            return View(dto);
        }

        TempData["SuccessMessage"] = $"Vehicle '{result.Data!.DisplayName}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return RedirectToAction(nameof(Index)); }

        var d = result.Data!;
        var dto = new UpdateVehicleDto
        {
            Id = d.Id,
            CustomerId = d.CustomerId,
            Make = d.Make,
            Model = d.Model,
            ManufacturingYear = d.ManufacturingYear,
            LicensePlate = d.LicensePlate,
            Vin = d.Vin,
            Color = d.Color,
            Mileage = d.Mileage,
            EngineNumber = d.EngineNumber,
            Notes = d.Notes
        };
        await PopulateCustomersDropdown(dto.CustomerId);
        return View(dto);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateVehicleDto dto)
    {
        if (!ModelState.IsValid) { await PopulateCustomersDropdown(dto.CustomerId); return View(dto); }

        var result = await _service.UpdateAsync(id, dto);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Failed to update vehicle.");
            await PopulateCustomersDropdown(dto.CustomerId);
            return View(dto);
        }

        TempData["SuccessMessage"] = "Vehicle updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return RedirectToAction(nameof(Index)); }
        return View(result.Data);
    }

    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return RedirectToAction(nameof(Delete), new { id }); }
        TempData["SuccessMessage"] = "Vehicle deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCustomersDropdown(int? selectedId = null)
    {
        var result = await _customerService.GetAllAsync();
        var customers = result.Success ? result.Data! : Enumerable.Empty<DTOs.Customers.CustomerDto>();
        ViewBag.Customers = new SelectList(customers, "Id", "FullName", selectedId);
    }
}
