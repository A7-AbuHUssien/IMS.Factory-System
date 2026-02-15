using IMS.Application.Common.Interfaces;
using IMS.Domain.DomainServices;
using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Sales.USeCases;

public class ConfirmUseCase
{
    private readonly IUnitOfWork _uow;
    private readonly ReservationDomainService _reservationDomain;

    public ConfirmUseCase(
        IUnitOfWork uow,
        ReservationDomainService reservationDomain)
    {
        _uow = uow;
        _reservationDomain = reservationDomain;
    }

    public async Task<bool> Execute(Guid orderId)
    {
        var order = await _uow.SalesOrders.GetOneAsync(
            e => e.Id == orderId,
            includes: [e => e.Items]);

        if (order == null) throw new BusinessException("Order not found");
        if (order.Status != SalesOrderStatus.Pending) throw new BusinessException("Only pending orders can be confirmed");
        if (!order.Items.Any()) throw new BusinessException("Order has no items");

        await _uow.BeginTransactionAsync();

        foreach (var item in order.Items)
        {
            var stocks = await _uow.Stocks
                .Query(tracked: true)
                .Where(s => s.ProductId == item.ProductId)
                .ToListAsync();

            if (stocks.Count == 0)
                throw new BusinessException($"No stock found for product {item.ProductId}");

            /* ===== DOMAIN LOGIC ===== */
            var allocations = _reservationDomain.Reserve(
                stocks,
                item.Quantity);
            /* ======================== */

            foreach (var allocation in allocations)
            {
                var reservation = new ReservationRequests
                {
                    OrderId = order.Id,
                    ProductId = item.ProductId,
                    WarehouseId = allocation.WarehouseId,
                    Quantity = allocation.Quantity,
                    Status = ReservationStatus.Reserved,
                    CreatedAt = DateTime.UtcNow
                };
                await _uow.ReservationRequests.CreateAsync(reservation);
            }

            foreach (var stock in stocks) _uow.Stocks.Update(stock);
        }

        order.RecalculateTotals();
        order.Status = SalesOrderStatus.Confirmed;

        _uow.SalesOrders.Update(order);

        await _uow.CommitAsync();
        await _uow.CommitTransactionAsync();

        return true;
    }
}