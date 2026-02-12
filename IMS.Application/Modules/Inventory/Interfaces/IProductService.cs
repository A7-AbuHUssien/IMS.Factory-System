using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Inventory.DTOs.Filters;
using IMS.Application.Modules.Inventory.DTOs.Product;

namespace IMS.Application.Modules.Inventory.Interfaces;

public interface IProductService
{
    Task<ProductDto> CreateAsync(CreateProductDto dto);
    Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto dto);
    Task DeleteAsync(Guid id);
    Task<ProductDto> GetByIdAsync(Guid id);
    Task<PaginatedApiResponse<ProductDto>> GetAllAsync(ProductFilterDto filter);
}