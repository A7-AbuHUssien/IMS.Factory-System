using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Inventory.DomainServices;
public static class StockGuard
{
    public static void EnsureNoNegative(decimal quantity)
    {
        if (quantity < 0) throw new BusinessException("Stock quantity cannot be negative");
    }

    public static void EnsureEnoughStock(decimal available, decimal requested)
    {
        if (available < requested)
            throw new BusinessException($"Insufficient stock. Available: {available}, Requested: {requested}");
    }

    public static void EnsurePositiveQuantity(decimal qty)
    {
        if (qty <= 0) throw new BusinessException("Quantity must be greater than zero");
    }

    public static void EnsureValidCost(decimal cost)
    {
        if (cost < 0) throw new BusinessException("Cost cannot be negative");
    }
}
