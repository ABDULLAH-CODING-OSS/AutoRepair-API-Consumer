using System.ComponentModel.DataAnnotations;

namespace ERP_Consumer.DTOs.Categories;

public class CategoryDto
{
    public int CategoryId { get; set; }
    public string CategoryName { get; set; } = string.Empty;
    public string? Description { get; set; }
    public DateTime CreatedAt { get; set; }

    public int Id => CategoryId;
    public string Name { get => CategoryName; set => CategoryName = value; }
}

public class CreateCategoryDto
{
    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100)]
    [Display(Name = "Category Name")]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public string Name { get => CategoryName; set => CategoryName = value; }
}

public class UpdateCategoryDto
{
    public int Id { get; set; }

    [Required(ErrorMessage = "Category name is required.")]
    [StringLength(100)]
    [Display(Name = "Category Name")]
    public string CategoryName { get; set; } = string.Empty;

    [StringLength(500)]
    public string? Description { get; set; }

    public string Name { get => CategoryName; set => CategoryName = value; }
}
