using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Inventory.DTOs.Filters;
using IMS.Application.Modules.Inventory.DTOs.Stock;

namespace IMS.Application.Modules.Inventory.Interfaces;

public interface IStockService
{
    Task ReceiveAsync(ReceiveStockDto dto);
    Task TransferAsync(TransferStockDto dto);
    Task AdjustAsync(AdjustStockDto dto);
    Task<StockDto?> GetSingleStockAsync(Guid warehouseId, Guid productId);
    Task<PaginatedApiResponse<StockDto>> GetStocksAsync(StockFilterDto filter);
}