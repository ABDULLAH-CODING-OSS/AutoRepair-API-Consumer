using ERP_Consumer.DTOs.Vehicles;
using ERP_Consumer.Services.Interfaces;

namespace ERP_Consumer.Services;

public class VehicleApiService : BaseApiService<VehicleDto, CreateVehicleDto, UpdateVehicleDto>, IVehicleApiService
{
    public VehicleApiService(HttpClient httpClient, ILogger<VehicleApiService> logger)
        : base(httpClient, logger, "VehiclesApi") { }
}
