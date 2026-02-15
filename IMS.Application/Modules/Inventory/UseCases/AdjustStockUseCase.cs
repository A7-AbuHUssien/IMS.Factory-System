using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Inventory.DTOs.Stock;
using IMS.Domain.DomainServices;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Inventory.UseCases;

public class AdjustStockUseCase : BaseStockUseCase
{
    private readonly StockTransactionFactory _factory;

    public AdjustStockUseCase(
        IUnitOfWork uow,
        StockTransactionFactory factory) : base(uow)
    {
        _factory = factory;
    }

    public async Task Execute(AdjustStockDto dto)
    {
        await _uow.BeginTransactionAsync();
        try
        {
            var stock = await GetStock(dto.ProductId, dto.WarehouseId);
            var diffQty = dto.ActualQuantity - stock.Quantity;
            if (diffQty == 0) return;
            
            var trans = _factory.CreateAdjustmentTransaction(dto.ProductId, dto.WarehouseId,diffQty,
                stock.AvgCost,dto.ActualQuantity, dto.Reason);
            
            var adjustment = new InventoryAdjustment()
            {
                ProductId = stock.ProductId,
                WarehouseId = stock.WarehouseId,
                QuantityBefore = stock.Quantity,
                QuantityAfter = dto.ActualQuantity,
                QuantityAdjusted = diffQty,
                CostImpact = (dto.ActualQuantity - stock.Quantity) * stock.AvgCost,
                Reason = dto.Reason
            };
            
             stock.Quantity = dto.ActualQuantity;
             stock.UpdatedAt = DateTime.Now;
             
             await _uow.StockTransactions.CreateAsync(trans);
             await _uow.InventoryAdjustments.CreateAsync(adjustment);
             _uow.Stocks.Update(stock);
             
             await _uow.CommitTransactionAsync();
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }
}
