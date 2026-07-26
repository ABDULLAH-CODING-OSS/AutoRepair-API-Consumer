using System.ComponentModel.DataAnnotations;

namespace ERP_Consumer.DTOs.Services;

public class ServiceDto
{
    public int ServiceId { get; set; }
    public string ServiceName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public decimal? StandardHours { get; set; }
    public decimal? FixedPrice { get; set; }
    public bool IsActive { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public int? CategoryId { get; set; }
    public DateTime CreatedAt { get; set; }

    public int Id => ServiceId;
    public string Name { get => ServiceName; set => ServiceName = value; }
    public decimal Price { get => FixedPrice ?? 0m; set => FixedPrice = value; }
    public int EstimatedMinutes { get => StandardHours.HasValue ? (int)Math.Round(StandardHours.Value) : 0; set => StandardHours = value; }
}

public class CreateServiceDto
{
    [Display(Name = "Category")]
    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "Service name is required.")]
    [StringLength(150)]
    [Display(Name = "Service Name")]
    public string ServiceName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0, 999999.99, ErrorMessage = "Price must be a positive value.")]
    [DataType(DataType.Currency)]
    public decimal FixedPrice { get; set; }

    [Required(ErrorMessage = "Estimated hours is required.")]
    [Range(0.01, 9999, ErrorMessage = "Enter a valid duration in hours.")]
    [Display(Name = "Standard Hours")]
    public decimal StandardHours { get; set; }

    public bool IsActive { get; set; } = true;
    public string Name { get => ServiceName; set => ServiceName = value; }
    public decimal Price { get => FixedPrice; set => FixedPrice = value; }
    public int EstimatedMinutes { get => (int)Math.Round(StandardHours); set => StandardHours = value; }
}

public class UpdateServiceDto
{
    public int Id { get; set; }

    [Display(Name = "Category")]
    public int? CategoryId { get; set; }

    [Required(ErrorMessage = "Service name is required.")]
    [StringLength(150)]
    [Display(Name = "Service Name")]
    public string ServiceName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    [Required(ErrorMessage = "Price is required.")]
    [Range(0, 999999.99, ErrorMessage = "Price must be a positive value.")]
    [DataType(DataType.Currency)]
    public decimal FixedPrice { get; set; }

    [Required(ErrorMessage = "Estimated hours is required.")]
    [Range(0.01, 9999, ErrorMessage = "Enter a valid duration in hours.")]
    [Display(Name = "Standard Hours")]
    public decimal StandardHours { get; set; }

    public bool IsActive { get; set; } = true;
    public string Name { get => ServiceName; set => ServiceName = value; }
    public decimal Price { get => FixedPrice; set => FixedPrice = value; }
    public int EstimatedMinutes { get => (int)Math.Round(StandardHours); set => StandardHours = value; }
}
