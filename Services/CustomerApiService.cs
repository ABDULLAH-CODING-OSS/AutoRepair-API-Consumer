using ERP_Consumer.DTOs.Customers;
using ERP_Consumer.Services.Interfaces;

namespace ERP_Consumer.Services;

public class CustomerApiService : BaseApiService<CustomerDto, CreateCustomerDto, UpdateCustomerDto>, ICustomerApiService
{
    public CustomerApiService(HttpClient httpClient, ILogger<CustomerApiService> logger)
        : base(httpClient, logger, "CustomersApi") { }
}
