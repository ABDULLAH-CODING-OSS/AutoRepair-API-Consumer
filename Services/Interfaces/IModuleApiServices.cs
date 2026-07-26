using ERP_Consumer.DTOs.Customers;
using ERP_Consumer.DTOs.Vehicles;
using ERP_Consumer.DTOs.Categories;
using ERP_Consumer.DTOs.Services;
using ERP_Consumer.DTOs.Parts;

namespace ERP_Consumer.Services.Interfaces;

public interface ICustomerApiService
    : IApiService<CustomerDto, CreateCustomerDto, UpdateCustomerDto> { }

public interface IVehicleApiService
    : IApiService<VehicleDto, CreateVehicleDto, UpdateVehicleDto> { }

public interface ICategoryApiService
    : IApiService<CategoryDto, CreateCategoryDto, UpdateCategoryDto> { }

public interface IServiceApiService
    : IApiService<ServiceDto, CreateServiceDto, UpdateServiceDto> { }

public interface IPartApiService
    : IApiService<PartDto, CreatePartDto, UpdatePartDto> { }
