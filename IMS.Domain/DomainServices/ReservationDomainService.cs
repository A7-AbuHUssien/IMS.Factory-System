using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Exceptions;

namespace IMS.Domain.DomainServices;

public class ReservationDomainService
{
    public List<ReservationAllocation> Reserve(List<Stock> stocks, decimal requestedQty)
    {
        if (requestedQty <= 0)
            throw new ArgumentException("Quantity must be greater than zero.");

        if (stocks == null || stocks.Count == 0)
            throw new InvalidOperationException("No stock available.");

        var ordered = stocks
            .OrderByDescending(s => s.AvailableQuantity)
            .ToList();

        var totalAvailable = ordered.Sum(s => s.AvailableQuantity);

        if (totalAvailable < requestedQty)
            throw new InvalidOperationException("Insufficient stock.");

        var remaining = requestedQty;
        var allocations = new List<ReservationAllocation>();

        foreach (var stock in ordered)
        {
            if (remaining <= 0)
                break;

            if (stock.AvailableQuantity <= 0)
                continue;

            var take = Math.Min(stock.AvailableQuantity, remaining);

            stock.ReservedQuantity += take;

            allocations.Add(new ReservationAllocation
            {
                WarehouseId = stock.WarehouseId,
                Quantity = take,
                UnitCost = stock.AvgCost
            });

            remaining -= take;
        }

        if (remaining > 0)
            throw new Exception("Reservation logic error.");

        return allocations;
    }

    /* ============================= */

    public void Release(List<Stock> stocks, List<ReservationAllocation> allocations)
    {
        if (stocks == null || allocations == null)
            throw new ArgumentNullException();

        foreach (var allocation in allocations)
        {
            var stock = stocks.FirstOrDefault(s => s.WarehouseId == allocation.WarehouseId);

            if (stock == null)
                throw new InvalidOperationException("Stock not found for release.");

            stock.ReservedQuantity -= allocation.Quantity;

            if (stock.ReservedQuantity < 0)
                throw new InvalidOperationException("Reserved quantity corrupted.");
        }
    }

    /* ============================= */

    public bool CanReserve(List<Stock> stocks, decimal qty)
    {
        return stocks.Sum(s => s.AvailableQuantity) >= qty;
    }

    /* ============================= */

    public decimal GetAvailable(List<Stock> stocks)
    {
        return stocks.Sum(s => s.AvailableQuantity);
    }

    /* ============================= */

    public void ValidateConsistency(List<Stock> stocks, List<ReservationAllocation> allocations)
    {
        var reservedFromStocks = stocks.Sum(s => s.ReservedQuantity);
        var reservedFromAllocations = allocations.Sum(a => a.Quantity);

        if (reservedFromStocks < reservedFromAllocations)
            throw new InvalidOperationException("Reservation inconsistency detected.");
    }
    public List<ReservationRequests> AllocateReservation(List<Stock> stocks, Guid orderId, decimal requestedQty)
    {
        if (requestedQty <= 0)
            throw new BusinessException("Invalid quantity");

        var result = new List<ReservationRequests>();

        var remaining = requestedQty;

        // sort stocks by best availability
        var orderedStocks = stocks
            .OrderByDescending(s => s.Quantity - s.ReservedQuantity)
            .ToList();

        foreach (var stock in orderedStocks)
        {
            if (remaining <= 0)
                break;

            var available = stock.Quantity - stock.ReservedQuantity;

            if (available <= 0)
                continue;

            var take = Math.Min(available, remaining);

            //---------------------------------------
            // update stock reserved
            //---------------------------------------
            stock.ReservedQuantity += take;

            //---------------------------------------
            // create reservation record
            //---------------------------------------
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

        //---------------------------------------
        // if still quantity missing
        //---------------------------------------
        if (remaining > 0)
            throw new BusinessException("Not enough stock available");

        return result;
    }
}