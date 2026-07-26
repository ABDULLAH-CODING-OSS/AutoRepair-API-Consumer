using ERP_Consumer.DTOs.Categories;
using ERP_Consumer.DTOs.Customers;
using ERP_Consumer.DTOs.Parts;
using ERP_Consumer.DTOs.Vehicles;

namespace ERP_Consumer.Helpers;

public static class EntityNameResolver
{
    public static void ApplyVehicleCustomerNames(IEnumerable<VehicleDto> vehicles, IEnumerable<CustomerDto> customers)
    {
        var customerNames = customers
            .Where(c => c != null)
            .ToDictionary(c => c.CustomerId, c => c.FullName, EqualityComparer<int>.Default);

        foreach (var vehicle in vehicles)
        {
            vehicle.CustomerName = ResolveName(vehicle.CustomerId, customerNames);
        }
    }

    public static void ApplyPartDisplayNames(IEnumerable<PartDto> parts, IEnumerable<CategoryDto> categories, IReadOnlyDictionary<int, string> supplierNames)
    {
        var categoryNames = categories
            .Where(c => c != null)
            .ToDictionary(c => c.CategoryId, c => c.CategoryName, EqualityComparer<int>.Default);

        foreach (var part in parts)
        {
            part.CategoryName = ResolveName(part.CategoryId, categoryNames);
            part.SupplierName = part.SupplierId.HasValue && supplierNames.TryGetValue(part.SupplierId.Value, out var supplierName)
                ? supplierName
                : null;
        }
    }

    private static string? ResolveName(int? id, IReadOnlyDictionary<int, string> names)
    {
        if (!id.HasValue)
            return null;

        return names.TryGetValue(id.Value, out var value) && !string.IsNullOrWhiteSpace(value)
            ? value
            : null;
    }
}
