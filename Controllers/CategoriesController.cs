using ERP_Consumer.DTOs.Categories;
using ERP_Consumer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP_Consumer.Controllers;

public class CategoriesController : Controller
{
    private readonly ICategoryApiService _service;
    private readonly ILogger<CategoriesController> _logger;

    public CategoriesController(ICategoryApiService service, ILogger<CategoriesController> logger)
    {
        _service = service;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var result = await _service.GetAllAsync();
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return View(Enumerable.Empty<CategoryDto>()); }

        var data = result.Data!;
        if (!string.IsNullOrWhiteSpace(search))
            data = data.Where(c => (c.CategoryName ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase));

        ViewBag.Search = search;
        return View(data);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return RedirectToAction(nameof(Index)); }
        return View(result.Data);
    }

    public IActionResult Create() => View(new CreateCategoryDto());

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCategoryDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        var result = await _service.CreateAsync(dto);
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Failed to create category."); return View(dto); }
        TempData["SuccessMessage"] = $"Category '{result.Data!.CategoryName}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return RedirectToAction(nameof(Index)); }
        var dto = new UpdateCategoryDto { Id = result.Data!.Id, CategoryName = result.Data.CategoryName, Description = result.Data.Description };
        return View(dto);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateCategoryDto dto)
    {
        if (!ModelState.IsValid) return View(dto);
        var result = await _service.UpdateAsync(id, dto);
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Failed to update category."); return View(dto); }
        TempData["SuccessMessage"] = "Category updated successfully.";
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
        TempData["SuccessMessage"] = "Category deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
