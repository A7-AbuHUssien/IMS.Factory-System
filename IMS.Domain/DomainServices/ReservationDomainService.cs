using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Exceptions;

namespace IMS.Domain.DomainServices;

public class ReservationDomainService
{

    public void Release(List<Stock> stocks, List<ReservationAllocation> allocations)
    {
        if (stocks == null || allocations == null) throw new ArgumentNullException();
        foreach (var allocation in allocations)
        {
            var stock = stocks.FirstOrDefault(s => s.WarehouseId == allocation.WarehouseId);
            if (stock == null) throw new BusinessException("Stock not found for release.");
            stock.ReservedQuantity -= allocation.Quantity;
            if (stock.ReservedQuantity < 0) throw new BusinessException("Reserved quantity corrupted.");
        }
    }
    public List<ReservationRequests> AllocateReservation(List<Stock> stocks, Guid orderId, decimal requestedQty)
    {
        if (requestedQty <= 0) throw new BusinessException("Invalid quantity");
        var result = new List<ReservationRequests>();
        var remaining = requestedQty;
        var orderedStocks = stocks.OrderByDescending(s => s.Quantity - s.ReservedQuantity).ToList();
        foreach (var stock in orderedStocks)
        {
            if (remaining <= 0) break;
            var available = stock.Quantity - stock.ReservedQuantity;
            if (available <= 0) continue;
            var take = Math.Min(available, remaining);
            stock.ReservedQuantity += take;
            result.Add(new ReservationRequests
            {
                OrderId = orderId,
                ProductId = stock.ProductId,
                WarehouseId = stock.WarehouseId,
                Quantity = take,
                Status = ReservationStatus.Reserved
            });
            remaining -= take;
        }
        if (remaining > 0) throw new BusinessException($"Not enough stock available");
        return result;
    }
}