using IMS.Application.Common.Interfaces;
using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;

namespace IMS.Application.Modules.Sales.USeCases;

public class CompleteUseCase
{
    private readonly IUnitOfWork _uow;

    public CompleteUseCase(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task<bool> Execute(Guid orderId)
    {
        var order = await _uow.SalesOrders
            .Query(true)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
            throw new BusinessException("Order not found");

        if (order.Status != SalesOrderStatus.Confirmed)
            throw new BusinessException("Only confirmed orders can be completed");

        var reservations = await _uow.ReservationRequests
            .Query(true)
            .Where(r => r.OrderId == orderId)
            .ToListAsync();

        if (!reservations.Any())
            throw new BusinessException("No reservations found for this order");

        await _uow.BeginTransactionAsync();

        try
        {
            foreach (var res in reservations)
            {
                var stock = await _uow.Stocks
                    .Query(true)
                    .FirstOrDefaultAsync(s => s.ProductId == res.ProductId && s.WarehouseId == res.WarehouseId);

                if (stock == null)
                    throw new BusinessException("Stock record missing");

                if (stock.ReservedQuantity < res.Quantity)
                    throw new BusinessException("Stock reservation mismatch");

                stock.Quantity -= res.Quantity;
                stock.ReservedQuantity -= res.Quantity;

                _uow.Stocks.Update(stock);
                
                await _uow.StockTransactions.CreateAsync(new StockTransaction
                {
                    ProductId = res.ProductId,
                    WarehouseId = res.WarehouseId,
                    Quantity = res.Quantity,
                    Type = StockTransactionType.Out,
                    Source = TransactionSource.Sale,
                    UnitCost = stock.AvgCost,
                    ReferenceId = order.Id,
                    BalanceAfter = stock.Quantity,
                    ReferenceType = "warehouse-to-customer"
                });
                res.Status = ReservationStatus.Consumed;
                _uow.ReservationRequests.Update(res);
            }

            order.Status = SalesOrderStatus.Delivered;
            _uow.SalesOrders.Update(order);

            await _uow.CommitAsync();
            await _uow.CommitTransactionAsync();

            return true;
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }
}