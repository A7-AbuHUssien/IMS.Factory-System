using IMS.Application.Common.Interfaces;
using IMS.Domain;
using IMS.Domain.DomainServices;
using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Sales.USeCases;

public class RemoveItemUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly ReservationDomainService _reservationDomain;

    public RemoveItemUseCase(
        IUnitOfWork uow,
        ReservationDomainService reservationDomain)
    {
        _uow = uow;
        _reservationDomain = reservationDomain;
    }

    public async Task<bool> Execute(Guid orderId, Guid itemId)
    {
        /* =======================
           LOAD ORDER
           ======================= */
        var order = await _uow.SalesOrders.GetOneAsync(
            o => o.Id == orderId,
            includes: [o => o.Items]);

        if (order == null)
            throw new BusinessException("Order not found");

        if (order.Status != SalesOrderStatus.Pending)
            throw new BusinessException("Only pending orders can be modified");

        /* =======================
           GET ITEM
           ======================= */
        var item = order.Items.FirstOrDefault(i => i.Id == itemId);

        if (item == null)
            throw new BusinessException("Item not found");

        /* =======================
           BEGIN TRANSACTION
           ======================= */
        await _uow.BeginTransactionAsync();

        /* =======================
           LOAD STOCKS
           ======================= */
        var stocks = await _uow.Stocks
            .Query(tracked: true)
            .Where(s => s.ProductId == item.ProductId)
            .ToListAsync();

        if (stocks.Count == 0)
            throw new BusinessException("Stock records missing");

        /* =======================
           LOAD RESERVATIONS
           ======================= */
        var reservations = await _uow.ReservationRequests
            .GetAsync(r =>
                r.OrderId == order.Id &&
                r.ProductId == item.ProductId &&
                r.Status == ReservationStatus.Reserved);

        /* =======================
           RELEASE DOMAIN LOGIC
           ======================= */
        var allocations = reservations.Select(r =>
            new ReservationAllocation(
                r.WarehouseId,
                r.Quantity,
                0)).ToList();

        _reservationDomain.Release(stocks, allocations);

        /* =======================
           UPDATE RESERVATIONS
           ======================= */
        foreach (var res in reservations)
        {
            res.Status = ReservationStatus.Released;
            res.UpdatedAt = DateTime.UtcNow;
        }

        /* =======================
           UPDATE STOCKS
           ======================= */
        foreach (var stock in stocks)
            _uow.Stocks.Update(stock);

        /* =======================
           REMOVE ITEM
           ======================= */
        order.Items.Remove(item);

        /* =======================
           RECALCULATE TOTALS
           ======================= */
        order.RecalculateTotals();

        /* =======================
           SAVE
           ======================= */
        _uow.SalesOrders.Update(order);
        _uow.SalesOrderItems.Delete(item);
        await _uow.CommitAsync();
        await _uow.CommitTransactionAsync();

        return true;
    }
}