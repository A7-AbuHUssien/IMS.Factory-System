using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Inventory.DTOs.Stock;
using IMS.Domain;
using IMS.Domain.DomainServices;
using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Sales.USeCases;

public class UpdateItemQuantityUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly ReservationDomainService _reservationService;

    public UpdateItemQuantityUseCase(IUnitOfWork uow, ReservationDomainService reservationService)
    {
        _uow = uow;
        _reservationService = reservationService;
    }

    public async Task<bool> Execute(Guid orderId, Guid itemId, decimal newQty)
    {
        if (newQty < 0) throw new BusinessException("Quantity cannot be negative");
        var order = await _uow.SalesOrders.GetOneAsync(e => e.Id == orderId, includes: [e => e.Items]);
        if (order == null) throw new BusinessException("Order not found");
        if (order.Status == SalesOrderStatus.Delivered || order.Status == SalesOrderStatus.Cancelled)
            throw new BusinessException("Order locked");
        var item = order.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null) throw new BusinessException("Item not found");

        var oldQty = item.Quantity;
        if (newQty == oldQty) return true;

        await _uow.BeginTransactionAsync();
        try
        {
            var diff = newQty - oldQty;
            if (order.Status == SalesOrderStatus.Pending)
            {
                item.Quantity = newQty;
                _uow.SalesOrderItems.Update(item);
                order.RecalculateTotals();
                _uow.SalesOrders.Update(order);
                await _uow.CommitAsync();
                await _uow.CommitTransactionAsync();
                return true;
            }
            //--------------------------------------------------
            // CASE 1 — REMOVE ITEM
            //--------------------------------------------------
            if (newQty == 0)
            {
                var reservations = await _uow.ReservationRequests.Query(true)
                    .Where(r => r.OrderId == orderId && r.ProductId == item.ProductId)
                    .OrderByDescending(r => r.CreatedAt)
                    .ToListAsync();

                foreach (var r in reservations)
                {
                    var stock = await _uow.Stocks.GetOneAsync(s =>
                        s.ProductId == r.ProductId && s.WarehouseId == r.WarehouseId);
                    stock.ReservedQuantity -= r.Quantity;

                    _uow.Stocks.Update(stock);
                    _uow.ReservationRequests.Delete(r);
                }

                _uow.SalesOrderItems.Delete(item);

                order.RecalculateTotals();
                _uow.SalesOrders.Update(order);

                await _uow.CommitAsync();
                await _uow.CommitTransactionAsync();

                return true;
            }

            //--------------------------------------------------
            // CASE 2 — INCREASE
            //--------------------------------------------------
            if (diff > 0)
            {
                var stocks = await _uow.Stocks
                    .Query(true)
                    .Where(s => s.ProductId == item.ProductId)
                    .OrderByDescending(s => s.Quantity - s.ReservedQuantity)
                    .ToListAsync();

                var allocations = _reservationService.AllocateReservation(stocks, orderId, diff);

                foreach (var alloc in allocations)
                    await _uow.ReservationRequests.CreateAsync(alloc);
                
                foreach (var s in stocks) _uow.Stocks.Update(s);
            }

            //--------------------------------------------------
            // CASE 3 — DECREASE
            //--------------------------------------------------
            if (diff < 0)
            {
                var needRelease = Math.Abs(diff);

                var reservations = await _uow.ReservationRequests
                    .Query(true)
                    .Where(r => r.OrderId == orderId && r.ProductId == item.ProductId)
                    .OrderBy(r => r.Quantity) 
                    .ToListAsync();

                if (!reservations.Any()) throw new BusinessException("No reservations found");

                var stocks = await _uow.Stocks
                    .Query(true)
                    .Where(s => s.ProductId == item.ProductId)
                    .ToListAsync();

                var allocations = new List<ReservationAllocation>();

                foreach (var r in reservations)
                {
                    if (needRelease <= 0) break;

                    var releaseQty = Math.Min(r.Quantity, needRelease);
                    allocations.Add(new ReservationAllocation(r.WarehouseId, releaseQty, 0));
                    r.Quantity -= releaseQty;

                    if (r.Quantity == 0)
                        _uow.ReservationRequests.Delete(r);
                    else
                        _uow.ReservationRequests.Update(r);
                    needRelease -= releaseQty;
                }

                if (needRelease > 0) throw new BusinessException("Reservation data corrupted");
                _reservationService.Release(stocks, allocations);
                foreach (var s in stocks) _uow.Stocks.Update(s);
            }
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }

        // UPDATE ITEM and RECALC TOTALS
        item.Quantity = newQty;
        _uow.SalesOrderItems.Update(item);

        order.RecalculateTotals();
        _uow.SalesOrders.Update(order);

        await _uow.CommitAsync();
        await _uow.CommitTransactionAsync();

        return true;
    }
}