using ERP_Consumer.DTOs.Services;
using ERP_Consumer.Services.Interfaces;

namespace ERP_Consumer.Services;

public class ServiceApiService : BaseApiService<ServiceDto, CreateServiceDto, UpdateServiceDto>, IServiceApiService
{
    public ServiceApiService(HttpClient httpClient, ILogger<ServiceApiService> logger)
        : base(httpClient, logger, "ServicesApi") { }
}
