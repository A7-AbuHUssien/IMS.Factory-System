using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.DTOs;
using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Inventory.DTOs.Filters;
using IMS.Application.Modules.Inventory.DTOs.Product;
using IMS.Application.Modules.Inventory.Interfaces;
using IMS.Domain.Entities;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Inventory.Services;

public class ProductService : IProductService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public ProductService(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<ProductDto> CreateAsync(CreateProductDto dto)
    {
        Product? entity = _mapper.Map<Product>(dto);
        await _uow.Products.CreateAsync(entity);
        await _uow.CommitAsync();
        return _mapper.Map<ProductDto>(entity);
    }

    public async Task<ProductDto> UpdateAsync(Guid id, UpdateProductDto dto)
    {
        var product = await _uow.Products.GetOneAsync(p => p.Id == id);
        if (product == null) throw new BusinessException("Product not found");
        dto.Id = product.Id;
        if (product == null)
            throw new BusinessException("Product not found");
        product = _mapper.Map(dto, product);
        _uow.Products.Update(product);
        await _uow.CommitAsync();
        return _mapper.Map<ProductDto>(product);
    }

    public async Task DeleteAsync(Guid id)
    {
        var product = await _uow.Products.GetOneAsync(p => p.Id == id);

        if (product == null)
            throw new BusinessException("Product not found");

        product.IsDeleted = true;
        _uow.Products.Update(product);
        await _uow.CommitAsync();
    }

    public async Task<ProductDto> GetByIdAsync(Guid id)
    {
        var product = await _uow.Products.GetOneAsync(p => p.Id == id);
        if (product == null)
            throw new BusinessException("Product not found");
        return _mapper.Map<ProductDto>(product);
    }

    public async Task<PaginatedApiResponse<ProductDto>> GetAllAsync(ProductFilterDto filter)
    {
        var query = _uow.Products.Query(tracked: false);

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(p =>
                p.Name.Contains(filter.Search) ||
                p.Description.Contains(filter.Search));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<ProductDto>(_mapper.ConfigurationProvider)
            .ToListAsync();

        return new PaginatedApiResponse<ProductDto>(items, filter.PageNumber, filter.PageSize, totalCount, "Products fetched");
        
    }

}
