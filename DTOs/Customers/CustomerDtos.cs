using System.ComponentModel.DataAnnotations;

namespace ERP_Consumer.DTOs.Customers;

public class CustomerDto
{
    public int CustomerId { get; set; }
    public int? CreatedByUserId { get; set; }
    public string FirstName { get; set; } = string.Empty;
    public string? LastName { get; set; }
    public string Phone { get; set; } = string.Empty;
    public string? Email { get; set; }
    public string? Address { get; set; }
    public string? City { get; set; }
    public DateTime CreatedAt { get; set; }

    public int Id => CustomerId;
    public string FullName => string.Join(" ", new[] { FirstName, LastName }.Where(x => !string.IsNullOrWhiteSpace(x))).Trim();
}

public class CreateCustomerDto
{
    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone is required.")]
    [Phone]
    [StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Address { get; set; }

    [StringLength(100)]
    [Display(Name = "City")]
    public string? City { get; set; }
}

public class UpdateCustomerDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "First name is required.")]
    [StringLength(100)]
    [Display(Name = "First Name")]
    public string FirstName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Last name is required.")]
    [StringLength(100)]
    [Display(Name = "Last Name")]
    public string LastName { get; set; } = string.Empty;

    [Required(ErrorMessage = "Phone is required.")]
    [Phone]
    [StringLength(30)]
    public string Phone { get; set; } = string.Empty;

    [Required(ErrorMessage = "Email is required.")]
    [EmailAddress]
    [StringLength(200)]
    public string Email { get; set; } = string.Empty;

    [StringLength(300)]
    public string? Address { get; set; }

    [StringLength(100)]
    [Display(Name = "City")]
    public string? City { get; set; }
}
