using System.ComponentModel.DataAnnotations;

namespace ERP_Consumer.DTOs.Parts;

public class PartDto
{
    public int PartId { get; set; }
    public int? CategoryId { get; set; }
    public int? SupplierId { get; set; }
    public string? Sku { get; set; }
    public string PartName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal CostPrice { get; set; }
    public decimal SalePrice { get; set; }
    public int? CurrentStock { get; set; }
    public int? ReorderLevel { get; set; }
    public string? Unit { get; set; }
    public string? RackLocation { get; set; }
    public bool IsActive { get; set; }
    public string? CategoryName { get; set; }
    public string? SupplierName { get; set; }
    public DateTime CreatedAt { get; set; }

    public int Id => PartId;
    public string Name { get => PartName; set => PartName = value; }
    public string PartNumber { get => Sku ?? string.Empty; set => Sku = value; }
    public int CurrentStockValue => CurrentStock ?? 0;
    public int ReorderLevelValue => ReorderLevel ?? 0;
    public bool IsActiveValue => IsActive;
}

public class CreatePartDto
{
    [Required(ErrorMessage = "Part name is required.")]
    [StringLength(150)]
    [Display(Name = "Part Name")]
    public string PartName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    [Display(Name = "SKU")]
    public string? Sku { get; set; }

    [Display(Name = "Category")]
    public int? CategoryId { get; set; }

    [Display(Name = "Supplier")]
    public int? SupplierId { get; set; }

    [Required(ErrorMessage = "Cost price is required.")]
    [Range(0, 999999.99, ErrorMessage = "Price must be a positive value.")]
    [Display(Name = "Cost Price")]
    [DataType(DataType.Currency)]
    public decimal CostPrice { get; set; }

    [Required(ErrorMessage = "Sale price is required.")]
    [Range(0, 999999.99, ErrorMessage = "Price must be a positive value.")]
    [Display(Name = "Sale Price")]
    [DataType(DataType.Currency)]
    public decimal SalePrice { get; set; }

    [Required(ErrorMessage = "Current stock is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be zero or more.")]
    [Display(Name = "Current Stock")]
    public int? CurrentStock { get; set; }

    [Required(ErrorMessage = "Reorder level is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Reorder level must be zero or more.")]
    [Display(Name = "Reorder Level")]
    public int? ReorderLevel { get; set; }

    [StringLength(50)]
    public string? Unit { get; set; }

    [StringLength(50)]
    [Display(Name = "Rack Location")]
    public string? RackLocation { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; } = true;
    public string Name { get => PartName; set => PartName = value; }
    public string PartNumber { get => Sku ?? string.Empty; set => Sku = value; }
}

public class UpdatePartDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Part name is required.")]
    [StringLength(150)]
    [Display(Name = "Part Name")]
    public string PartName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [StringLength(50)]
    [Display(Name = "SKU")]
    public string? Sku { get; set; }

    [Display(Name = "Category")]
    public int? CategoryId { get; set; }

    [Display(Name = "Supplier")]
    public int? SupplierId { get; set; }

    [Required(ErrorMessage = "Cost price is required.")]
    [Range(0, 999999.99, ErrorMessage = "Price must be a positive value.")]
    [Display(Name = "Cost Price")]
    [DataType(DataType.Currency)]
    public decimal CostPrice { get; set; }

    [Required(ErrorMessage = "Sale price is required.")]
    [Range(0, 999999.99, ErrorMessage = "Price must be a positive value.")]
    [Display(Name = "Sale Price")]
    [DataType(DataType.Currency)]
    public decimal SalePrice { get; set; }

    [Required(ErrorMessage = "Current stock is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Stock must be zero or more.")]
    [Display(Name = "Current Stock")]
    public int? CurrentStock { get; set; }

    [Required(ErrorMessage = "Reorder level is required.")]
    [Range(0, int.MaxValue, ErrorMessage = "Reorder level must be zero or more.")]
    [Display(Name = "Reorder Level")]
    public int? ReorderLevel { get; set; }

    [StringLength(50)]
    public string? Unit { get; set; }

    [StringLength(50)]
    [Display(Name = "Rack Location")]
    public string? RackLocation { get; set; }

    [Display(Name = "Active")]
    public bool IsActive { get; set; }
    public string Name { get => PartName; set => PartName = value; }
    public string PartNumber { get => Sku ?? string.Empty; set => Sku = value; }
}
