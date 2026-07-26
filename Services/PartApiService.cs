using ERP_Consumer.DTOs.Parts;
using ERP_Consumer.Services.Interfaces;

namespace ERP_Consumer.Services;

public class PartApiService : BaseApiService<PartDto, CreatePartDto, UpdatePartDto>, IPartApiService
{
    public PartApiService(HttpClient httpClient, ILogger<PartApiService> logger)
        : base(httpClient, logger, "PartsApi") { }
}
