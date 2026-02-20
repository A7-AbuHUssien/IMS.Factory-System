using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Sales.DTOs.Customer;
using IMS.Application.Modules.Sales.Filters;
using IMS.Application.Modules.Sales.Interfaces;
using IMS.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers.Sales
{
    [Route("api/sales/[controller]")]
    [Authorize(Roles = $"{AppRoles.Admin},{AppRoles.Manager},{AppRoles.User}")]
    [ApiController]
    public class CustomersController : ControllerBase
    {
        private readonly ICustomerService _customerService;
        public CustomersController(ICustomerService customerService)
        {
            _customerService = customerService;
        }

        [HttpGet]
        [Route("{id:guid}")]
        public async Task<ActionResult<CustomerDto>> GetCustomerByIdAsync(Guid id)
        {
           return await _customerService.GetCustomerByIdAsync(id);
        }

        [HttpGet]
        public async Task<ActionResult<PaginatedApiResponse<CustomerDto>?>> GetAllCustomersAsync([FromQuery]CustomerFilter filter)
        {
            return await _customerService.GetAllCustomersAsync(filter);
        }

        [HttpPost]
        public async Task<ApiResponse<CustomerDto?>> CreateCustomerAsync([FromBody]CreateCustomerDto dto)
        {
            return new ApiResponse<CustomerDto?>(await _customerService.CreateCustomerAsync(dto), "Created Successfully");
        }
    }
}
