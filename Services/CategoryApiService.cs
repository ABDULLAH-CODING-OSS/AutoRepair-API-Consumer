using ERP_Consumer.DTOs.Categories;
using ERP_Consumer.Services.Interfaces;

namespace ERP_Consumer.Services;

public class CategoryApiService : BaseApiService<CategoryDto, CreateCategoryDto, UpdateCategoryDto>, ICategoryApiService
{
    public CategoryApiService(HttpClient httpClient, ILogger<CategoryApiService> logger)
        : base(httpClient, logger, "CategoriesApi") { }
}
