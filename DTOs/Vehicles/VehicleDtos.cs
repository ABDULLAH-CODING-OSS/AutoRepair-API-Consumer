using System.ComponentModel.DataAnnotations;

namespace ERP_Consumer.DTOs.Vehicles;

public class VehicleDto
{
    public int VehicleId { get; set; }
    public int CustomerId { get; set; }
    public int? CreatedByUserId { get; set; }
    public string LicensePlate { get; set; } = string.Empty;
    public string? Vin { get; set; }
    public string Make { get; set; } = string.Empty;
    public string Model { get; set; } = string.Empty;
    public int? ManufacturingYear { get; set; }
    public string? Color { get; set; }
    public int? Mileage { get; set; }
    public string? EngineNumber { get; set; }
    public string? Notes { get; set; }
    public DateTime CreatedAt { get; set; }
    public string? CustomerName { get; set; }

    public int Id => VehicleId;
    public string DisplayName => string.Join(" ", new[] { ManufacturingYear?.ToString(), Make, Model }.Where(x => !string.IsNullOrWhiteSpace(x))).Trim();
}

public class CreateVehicleDto
{
    [Required(ErrorMessage = "Customer is required.")]
    [Display(Name = "Customer")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "License plate is required.")]
    [StringLength(20)]
    [Display(Name = "License Plate")]
    public string LicensePlate { get; set; } = string.Empty;

    [StringLength(17)]
    public string? Vin { get; set; }

    [Required(ErrorMessage = "Make is required.")]
    [StringLength(100)]
    public string Make { get; set; } = string.Empty;

    [Required(ErrorMessage = "Model is required.")]
    [StringLength(100)]
    public string Model { get; set; } = string.Empty;

    [Display(Name = "Manufacturing Year")]
    [Range(1886, 2100, ErrorMessage = "Enter a valid year.")]
    public int? ManufacturingYear { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    [Range(0, 9999999, ErrorMessage = "Enter a valid mileage.")]
    public int? Mileage { get; set; }

    [StringLength(50)]
    [Display(Name = "Engine Number")]
    public string? EngineNumber { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public int? Year { get => ManufacturingYear; set => ManufacturingYear = value; }
}

public class UpdateVehicleDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Customer is required.")]
    [Display(Name = "Customer")]
    public int CustomerId { get; set; }

    [Required(ErrorMessage = "License plate is required.")]
    [StringLength(20)]
    [Display(Name = "License Plate")]
    public string LicensePlate { get; set; } = string.Empty;

    [StringLength(17)]
    public string? Vin { get; set; }

    [Required(ErrorMessage = "Make is required.")]
    [StringLength(100)]
    public string Make { get; set; } = string.Empty;

    [Required(ErrorMessage = "Model is required.")]
    [StringLength(100)]
    public string Model { get; set; } = string.Empty;

    [Display(Name = "Manufacturing Year")]
    [Range(1886, 2100, ErrorMessage = "Enter a valid year.")]
    public int? ManufacturingYear { get; set; }

    [StringLength(50)]
    public string? Color { get; set; }

    [Range(0, 9999999, ErrorMessage = "Enter a valid mileage.")]
    public int? Mileage { get; set; }

    [StringLength(50)]
    [Display(Name = "Engine Number")]
    public string? EngineNumber { get; set; }

    [StringLength(500)]
    public string? Notes { get; set; }

    public int? Year { get => ManufacturingYear; set => ManufacturingYear = value; }
}
