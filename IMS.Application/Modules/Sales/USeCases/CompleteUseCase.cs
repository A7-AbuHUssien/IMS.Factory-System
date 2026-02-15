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
        //-----------------------------------------
        // LOAD ORDER
        //-----------------------------------------
        var order = await _uow.SalesOrders
            .Query(true)
            .FirstOrDefaultAsync(o => o.Id == orderId);

        if (order == null)
            throw new BusinessException("Order not found");

        //-----------------------------------------
        // VALIDATE STATUS
        //-----------------------------------------
        if (order.Status != SalesOrderStatus.Confirmed)
            throw new BusinessException("Only confirmed orders can be completed");

        //-----------------------------------------
        // LOAD RESERVATIONS
        //-----------------------------------------
        var reservations = await _uow.ReservationRequests
            .Query(true)
            .Where(r => r.OrderId == orderId && r.IsDeleted == false)
            .ToListAsync();

        if (!reservations.Any())
            throw new BusinessException("No reservations found for this order");

        //-----------------------------------------
        // TRANSACTION START
        //-----------------------------------------
        await _uow.BeginTransactionAsync();

        try
        {
            foreach (var res in reservations)
            {
                //---------------------------------
                // LOAD STOCK
                //---------------------------------
                var stock = await _uow.Stocks
                    .Query(true)
                    .FirstOrDefaultAsync(s =>
                        s.ProductId == res.ProductId &&
                        s.WarehouseId == res.WarehouseId);

                if (stock == null)
                    throw new BusinessException("Stock record missing");

                //---------------------------------
                // VALIDATE RESERVED
                //---------------------------------
                if (stock.ReservedQuantity < res.Quantity)
                    throw new BusinessException("Stock reservation mismatch");

                //---------------------------------
                // APPLY STOCK MOVEMENT
                //---------------------------------
                stock.Quantity -= res.Quantity;
                stock.ReservedQuantity -= res.Quantity;

                _uow.Stocks.Update(stock);

                //---------------------------------
                // CREATE STOCK TRANSACTION
                //---------------------------------
                await _uow.StockTransactions.CreateAsync(new StockTransaction
                {
                    ProductId = res.ProductId,
                    WarehouseId = res.WarehouseId,
                    Quantity = res.Quantity,
                    Type = StockTransactionType.Out,
                    Source = TransactionSource.Sale,
                    ReferenceId = order.Id,
                    BalanceAfter = stock.Quantity,
                    ReferenceType = "warehouse-to-customer"
                });

                //---------------------------------
                // DELETE RESERVATION
                //---------------------------------
                res.Status = ReservationStatus.Consumed;
                _uow.ReservationRequests.Update(res);
            }

            //-----------------------------------------
            // UPDATE ORDER STATUS
            //-----------------------------------------
            order.Status = SalesOrderStatus.Delivered;
            _uow.SalesOrders.Update(order);

            //-----------------------------------------
            // SAVE
            //-----------------------------------------
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