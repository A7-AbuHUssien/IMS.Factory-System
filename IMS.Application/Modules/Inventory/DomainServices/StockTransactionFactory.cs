namespace IMS.Application.Modules.Inventory.DomainServices;

using IMS.Domain.Entities;
using IMS.Domain.Enums;

public class StockTransactionFactory
{
    public StockTransaction CreateIn(Guid productId, Guid warehouseId, decimal quantity,
        decimal unitCost, decimal balanceAfter, string reference, TransactionSource source)
    {
        return new StockTransaction
        {
            ProductId = productId,
            WarehouseId = warehouseId,

            Type = StockTransactionType.In,
            Source = source,

            Quantity = quantity,
            UnitCost = unitCost,
            UnitPrice = 0,

            BalanceAfter = balanceAfter,
            TransactionDate = DateTime.UtcNow,

            ReferenceType = reference
        };
    }

    public StockTransaction CreateOut(Guid productId, Guid warehouseId, decimal quantity,
        decimal unitCost, decimal unitPrice, decimal balanceAfter, string reference, TransactionSource source)
    {
        return new StockTransaction
        {
            ProductId = productId,
            WarehouseId = warehouseId,

            Type = StockTransactionType.Out,
            Source = source,

            Quantity = quantity,
            UnitCost = unitCost,
            UnitPrice = unitPrice,

            BalanceAfter = balanceAfter,
            TransactionDate = DateTime.UtcNow,

            ReferenceType = reference
        };
    }

    public StockTransaction CreateAdjustmentTransaction(Guid productId, Guid warehouseId, decimal quantity,
        decimal unitCost, decimal balanceAfter, string reason)
    {
        return new StockTransaction
        {
            ProductId = productId,
            WarehouseId = warehouseId,
            
            Quantity = quantity,
            UnitCost = unitCost,
            
            BalanceAfter = balanceAfter,
            
            TransactionDate = DateTime.UtcNow,
            
            ReferenceType = reason,
            
            Type = StockTransactionType.Adjust,
            Source = TransactionSource.InventoryAdjustment,
        };
    }
}