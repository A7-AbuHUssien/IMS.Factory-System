using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Sales.DTOs.Customer;
using IMS.Application.Modules.Sales.Filters;

namespace IMS.Application.Modules.Sales.Interfaces;

public interface ICustomerService
{
    Task<CustomerDto> GetCustomerByIdAsync(Guid id);
    Task<PaginatedApiResponse<CustomerDto>?> GetAllCustomersAsync(CustomerFilter filter);
    Task<CustomerDto?> CreateCustomerAsync(CreateCustomerDto customerDto);
    Task<CustomerDto?> UpdateCustomerAsync(Guid id, UpdateCustomerDto customerDto);
    Task DeleteCustomerAsync(Guid id);
}