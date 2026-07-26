using ERP_Consumer.DTOs.Parts;
using ERP_Consumer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;

namespace ERP_Consumer.Controllers;

public class PartsController : Controller
{
    private readonly IPartApiService _service;
    private readonly ICategoryApiService _categoryService;
    private readonly ILogger<PartsController> _logger;

    public PartsController(IPartApiService service, ICategoryApiService categoryService, ILogger<PartsController> logger)
    {
        _service = service;
        _categoryService = categoryService;
        _logger = logger;
    }

    public async Task<IActionResult> Index(string? search)
    {
        var result = await _service.GetAllAsync();
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return View(Enumerable.Empty<PartDto>()); }

        var data = result.Data!;
        if (!string.IsNullOrWhiteSpace(search))
            data = data.Where(p => (p.PartName ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase) ||
                                   (p.Sku ?? string.Empty).Contains(search, StringComparison.OrdinalIgnoreCase));

        ViewBag.Search = search;
        return View(data);
    }

    public async Task<IActionResult> Details(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return RedirectToAction(nameof(Index)); }
        return View(result.Data);
    }

    public IActionResult Create() 
    { 
        PopulateDropdowns();
        return View(new CreatePartDto()); 
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreatePartDto dto)
    {
        if (!ModelState.IsValid) { PopulateDropdowns(dto.CategoryId, dto.SupplierId); return View(dto); }
        var result = await _service.CreateAsync(dto);
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Failed to create part."); PopulateDropdowns(dto.CategoryId, dto.SupplierId); return View(dto); }
        TempData["SuccessMessage"] = $"Part '{result.Data!.PartName}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    public async Task<IActionResult> Edit(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success) { TempData["ErrorMessage"] = result.ErrorMessage; return RedirectToAction(nameof(Index)); }
        var d = result.Data!;
        var dto = new UpdatePartDto 
        { 
            Id = d.Id, 
            PartName = d.PartName, 
            Description = d.Description, 
            Sku = d.Sku, 
            CategoryId = d.CategoryId,
            SupplierId = d.SupplierId,
            CostPrice = d.CostPrice, 
            SalePrice = d.SalePrice, 
            CurrentStock = d.CurrentStock,
            ReorderLevel = d.ReorderLevel,
            Unit = d.Unit,
            RackLocation = d.RackLocation,
            IsActive = d.IsActiveValue
        };
        PopulateDropdowns(dto.CategoryId, dto.SupplierId);
        return View(dto);
    }

    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdatePartDto dto)
    {
        if (!ModelState.IsValid) { PopulateDropdowns(dto.CategoryId, dto.SupplierId); return View(dto); }
        var result = await _service.UpdateAsync(id, dto);
        if (!result.Success) { ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Failed to update part."); PopulateDropdowns(dto.CategoryId, dto.SupplierId); return View(dto); }
        TempData["SuccessMessage"] = "Part updated successfully.";
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
        TempData["SuccessMessage"] = "Part deleted successfully.";
        return RedirectToAction(nameof(Index));
    }

    private void PopulateDropdowns(int? categoryId = null, int? supplierId = null)
    {
        PopulateCategoriesDropdown(categoryId);
        PopulateSuppliersDropdown(supplierId);
    }

    private void PopulateCategoriesDropdown(int? selectedId = null)
    {
        var result = _categoryService.GetAllAsync().Result;
        var categories = result.Success ? result.Data! : Enumerable.Empty<DTOs.Categories.CategoryDto>();
        ViewBag.Categories = new SelectList(categories, "Id", "Name", selectedId);
    }

    private void PopulateSuppliersDropdown(int? selectedId = null)
    {
        var suppliers = new[]
        {
            new { Id = 1, Name = "Auto Parts Express" },
            new { Id = 2, Name = "North Star Suppliers" },
            new { Id = 3, Name = "Genuine Auto Spares" }
        };

        ViewBag.Suppliers = new SelectList(suppliers, "Id", "Name", selectedId);
    }
}
