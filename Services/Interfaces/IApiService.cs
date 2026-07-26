using ERP_Consumer.Helpers;

namespace ERP_Consumer.Services.Interfaces;

public interface IApiService<TDto, TCreate, TUpdate>
    where TDto : class
    where TCreate : class
    where TUpdate : class
{
    Task<ApiResponse<IEnumerable<TDto>>> GetAllAsync();
    Task<ApiResponse<TDto>> GetByIdAsync(int id);
    Task<ApiResponse<TDto>> CreateAsync(TCreate dto);
    Task<ApiResponse<TDto>> UpdateAsync(int id, TUpdate dto);
    Task<ApiResponse<bool>> DeleteAsync(int id);
}
