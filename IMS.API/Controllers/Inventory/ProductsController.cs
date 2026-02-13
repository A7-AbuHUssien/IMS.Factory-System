using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Inventory.DTOs.Filters;
using IMS.Application.Modules.Inventory.DTOs.Product;
using IMS.Application.Modules.Inventory.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers.Inventory;

[Route("api/inventory/[controller]")]
[ApiController]
public class ProductsController : ControllerBase
{
    private readonly IProductService  _productService;

    public ProductsController(IProductService productService)
    {
        _productService = productService;
    }

    [HttpGet("all")]
    public async Task<PaginatedApiResponse<ProductDto>>Get([FromQuery] ProductFilterDto filter)
    {
        return await _productService.GetAllAsync(filter);
    }

    [HttpGet("{id}")]
    public async Task<ApiResponse<ProductDto>> Get(Guid id)
    {
        return new ApiResponse<ProductDto>(await _productService.GetByIdAsync(id));
    }

    [HttpPost("create")]
    public async Task<ApiResponse<ProductDto>> Create([FromBody] CreateProductDto dto)
    {
        return new ApiResponse<ProductDto>(await _productService.CreateAsync(dto), "Created Successfully");
    }

    [HttpPut("update")]
    public async Task<ApiResponse<ProductDto>> Update(Guid id, [FromBody] UpdateProductDto dto)
    {
        return new ApiResponse<ProductDto>(await _productService.UpdateAsync(id,dto),"Updated Successfully");
    }
}