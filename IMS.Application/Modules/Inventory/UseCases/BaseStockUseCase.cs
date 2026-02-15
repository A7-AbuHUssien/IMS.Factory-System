using IMS.Application.Common.Interfaces;
using IMS.Domain.Entities;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Inventory.UseCases;

public abstract class BaseStockUseCase
{
    protected readonly IUnitOfWork _uow;

    protected BaseStockUseCase(IUnitOfWork uow)
    {
        _uow = uow;
    }

    protected async Task<Stock> GetOrCreateStock(Guid productId, Guid warehouseId)
    {
        var stock = await _uow.Stocks.GetOneAsync(
            s => s.ProductId == productId &&
                 s.WarehouseId == warehouseId);

        if (stock != null)
            return stock;

        stock = new Stock
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            Quantity = 0,
            ReservedQuantity = 0,
            AvgCost = 0,
        };
        stock =  await _uow.Stocks.CreateAsync(stock);
        return stock;
    }
    protected async Task<Stock> GetStock(Guid productId, Guid warehouseId)
    {
        var stock = await _uow.Stocks.GetOneAsync(
            s => s.ProductId == productId &&
                 s.WarehouseId == warehouseId);

        if (stock == null)
            throw new BusinessException(
                $"No stock record found for product {productId} in warehouse {warehouseId}");

        return stock;
    }
    protected async Task EnsureProductAndWarehouseExist(Guid productId, Guid warehouseId)
    {
        var product = await _uow.Products.GetOneAsync(p => p.Id == productId);
        if (product == null)
            throw new BusinessException("Product not found");

        var warehouse = await _uow.Warehouses.GetOneAsync(w => w.Id == warehouseId);
        if (warehouse == null)
            throw new BusinessException("Warehouse not found");
    }

    
}
