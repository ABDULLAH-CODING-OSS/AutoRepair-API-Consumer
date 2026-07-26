using ERP_Consumer.DTOs.Services;
using ERP_Consumer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_Consumer.Controllers;

public class ServicesController : Controller
{
    private readonly IServiceApiService _service;
    private readonly ICategoryApiService _categoryService;
    private readonly ILogger<ServicesController> _logger;

    public ServicesController(IServiceApiService service, ICategoryApiService categoryService, ILogger<ServicesController> logger)
    {
        _service = service;
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var result = await _service.GetAllAsync();
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return View(Enumerable.Empty<ServiceDto>()); }

        var data = result.Data!;
        if (!string.IsNullOrWhiteSpace(search))
            data = data.Where(s => (s.ServiceName ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                   (s.CategoryName ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase));

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
        await PopulateCategoriesDropdown();
        return View(new CreateServiceDto());
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateServiceDto dto)
    {
        if (!ModelState.IsValid) { await PopulateCategoriesDropdown(dto.CategoryId); return View(dto); }
        var result = await _service.CreateAsync(dto);
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Failed to create service."); await PopulateCategoriesDropdown(dto.CategoryId); return View(dto); }
        TempData["SuccessMessage"] = $"Service '{result.Data!.ServiceName}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return RedirectToAction(nameof(Index)); }
        var d = result.Data!;
        var dto = new UpdateServiceDto { Id = d.Id, CategoryId = 1, ServiceName = d.ServiceName, Description = d.Description, FixedPrice = d.FixedPrice ?? 0m, StandardHours = d.StandardHours ?? 0m };
        await PopulateCategoriesDropdown(dto.CategoryId);
        return View(dto);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateServiceDto dto)
    {
        if (!ModelState.IsValid) { await PopulateCategoriesDropdown(dto.CategoryId); return View(dto); }
        var result = await _service.UpdateAsync(id, dto);
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Failed to update service."); await PopulateCategoriesDropdown(dto.CategoryId); return View(dto); }
        TempData["SuccessMessage"] = "Service updated successfully.";
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
        TempData["SuccessMessage"] = "Service deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private async Task PopulateCategoriesDropdown(int? selectedId = null)
    {
        var result = await _categoryService.GetAllAsync();
        var categories = result.Success ? result.Data! : Enumerable.Empty<DTOs.Categories.CategoryDto>();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
    }
}
