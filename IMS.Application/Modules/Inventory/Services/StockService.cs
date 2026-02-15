using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.DTOs;
using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Inventory.DTOs.Filters;
using IMS.Application.Modules.Inventory.DTOs.Product;
using IMS.Application.Modules.Inventory.DTOs.Stock;
using IMS.Application.Modules.Inventory.Interfaces;
using IMS.Application.Modules.Inventory.UseCases;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Inventory.Services;

public class StockService : IStockService
{
    private readonly ReceiveStockUseCase _receive;
    private readonly TransferStockUseCase _transfer;
    private readonly AdjustStockUseCase _adjust;
    private readonly IUnitOfWork _unitOfWork;
    private readonly IMapper _mapper;

    public StockService(ReceiveStockUseCase receive, TransferStockUseCase transfer, AdjustStockUseCase adjust,
        IUnitOfWork unitOfWork, IMapper mapper)
    {
        _receive = receive;
        _transfer = transfer;
        _adjust = adjust;
        _unitOfWork = unitOfWork;
        _mapper = mapper;
    }

    public Task ReceiveAsync(ReceiveStockDto dto) => _receive.Execute(dto);

    public Task TransferAsync(TransferStockDto dto) => _transfer.Execute(dto);

    public Task AdjustAsync(AdjustStockDto dto) => _adjust.Execute(dto);

    public async Task<StockDto?> GetSingleStockAsync(Guid stockId)
    {
/*  
        var result = await _unitOfWork.Stocks.GetOneAsync
            (expression: s => s.ProductId == productId && s.WarehouseId == warehouseId,
                includes: [s => s.Warehouse, s => s.Product],tracked:false);
        if (result == null)
            throw new BusinessException("Stock not found");
        return _mapper.Map<StockDto>(result);
*/
       var query = _unitOfWork.Stocks.Query();
       query = query.Where(e => e.Id == stockId);
       var res1 = await query
            .ProjectTo<StockDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
       return res1;
    }

    public async Task<PaginatedApiResponse<StockDto>> GetStocksAsync(StockFilterDto filter)
    {
        var query = _unitOfWork.Stocks.Query(tracked: false);
        if (!String.IsNullOrWhiteSpace(filter.Search))
            query = query.Where(e => e.Product.Name.Contains(filter.Search) ||
                                     e.Product.SKU.Contains(filter.Search) ||
                                     e.Product.Description.Contains(filter.Search));
        var totalCount = await query.CountAsync();
        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<StockDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        return new PaginatedApiResponse<StockDto>(items, filter.PageNumber, filter.PageSize, totalCount, "Products fetched");
    }
    
}