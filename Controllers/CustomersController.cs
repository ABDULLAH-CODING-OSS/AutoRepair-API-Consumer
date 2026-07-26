using ERP_Consumer.DTOs.Customers;
using ERP_Consumer.Services.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace ERP_Consumer.Controllers;

public class CustomersController : Controller
{
    private readonly ICustomerApiService _service;
    private readonly ILogger<CustomersController> _logger;

    public CustomersController(ICustomerApiService service, ILogger<CustomersController> logger)
    {
        _service = service;
        _logger = logger;
    }

    // GET /Customers
    public async Task<IActionResult> Index(string? search)
    {
        var result = await _service.GetAllAsync();
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return View(Enumerable.Empty<CustomerDto>());
        }

        var data = result.Data!;
        if (!string.IsNullOrWhiteSpace(search))
        {
            var lower = search.ToLower();
            data = data.Where(c =>
                (c.FirstName ?? string.Empty).Contains(lower, StringComparison.OrdinalIgnoreCase) ||
                (c.LastName ?? string.Empty).Contains(lower, StringComparison.OrdinalIgnoreCase) ||
                (c.Email ?? string.Empty).Contains(lower, StringComparison.OrdinalIgnoreCase) ||
                (c.Phone ?? string.Empty).Contains(lower, StringComparison.OrdinalIgnoreCase));
        }

        ViewBag.Search = search;
        return View(data);
    }

    // GET /Customers/Details/5
    public async Task<IActionResult> Details(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    // GET /Customers/Create
    public IActionResult Create() => View(new CreateCustomerDto());

    // POST /Customers/Create
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Create(CreateCustomerDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var result = await _service.CreateAsync(dto);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Failed to create customer.");
            return View(dto);
        }

        TempData["SuccessMessage"] = $"Customer '{result.Data!.FullName}' created successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET /Customers/Edit/5
    public async Task<IActionResult> Edit(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }

        var dto = new UpdateCustomerDto
        {
            Id = result.Data!.Id,
            FirstName = result.Data.FirstName,
            LastName = result.Data.LastName ?? string.Empty,
            Email = result.Data.Email ?? string.Empty,
            Phone = result.Data.Phone,
            Address = result.Data.Address,
            City = result.Data.City
        };
        return View(dto);
    }

    // POST /Customers/Edit/5
    [HttpPost, ValidateAntiForgeryToken]
    public async Task<IActionResult> Edit(int id, UpdateCustomerDto dto)
    {
        if (!ModelState.IsValid) return View(dto);

        var result = await _service.UpdateAsync(id, dto);
        if (!result.Success)
        {
            ModelState.AddModelError(string.Empty, result.ErrorMessage ?? "Failed to update customer.");
            return View(dto);
        }

        TempData["SuccessMessage"] = "Customer updated successfully.";
        return RedirectToAction(nameof(Index));
    }

    // GET /Customers/Delete/5
    public async Task<IActionResult> Delete(int id)
    {
        var result = await _service.GetByIdAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Index));
        }
        return View(result.Data);
    }

    // POST /Customers/Delete/5
    [HttpPost, ActionName("Delete"), ValidateAntiForgeryToken]
    public async Task<IActionResult> DeleteConfirmed(int id)
    {
        var result = await _service.DeleteAsync(id);
        if (!result.Success)
        {
            TempData["ErrorMessage"] = result.ErrorMessage;
            return RedirectToAction(nameof(Delete), new { id });
        }

        TempData["SuccessMessage"] = "Customer deleted successfully.";
        return RedirectToAction(nameof(Index));
    }
}
