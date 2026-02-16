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
        // Order and Item Validation
        var order = await _uow.SalesOrders.GetOneAsync(o => o.Id == orderId, includes: [o => o.Items]);
        if (order == null) throw new BusinessException("Order not found");
        if (order.Status != SalesOrderStatus.Pending)
            throw new BusinessException("Only pending orders can be modified");
        var item = order.Items.FirstOrDefault(i => i.Id == itemId);
        if (item == null) throw new BusinessException("Item not found");


        await _uow.BeginTransactionAsync();
        try
        {
            order.Items.Remove(item);
            _uow.SalesOrderItems.Delete(item);
            order.RecalculateTotals();
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