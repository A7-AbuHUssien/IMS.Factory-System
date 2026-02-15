using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Inventory.Interfaces;
using IMS.Domain.DomainServices;
using IMS.Domain.Enums;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Sales.USeCases;

public class CancelUseCase
{
    private readonly IUnitOfWork _uow;
    public CancelUseCase(IUnitOfWork unitOfWork)
    {
        _uow = unitOfWork;
    }

    public async Task<bool> Execute(Guid orderId)
    {
        var order = await _uow.SalesOrders.GetOneAsync(expression:e => e.Id == orderId,includes:[e=>e.Items]);
        if (order == null) throw new BusinessException("Order not found");
        if (order.Status != SalesOrderStatus.Pending)
            throw new BusinessException("Order is already in progress");
        
        order.Status = SalesOrderStatus.Cancelled;
        
        _uow.SalesOrders.Update(order);
        await _uow.CommitAsync();
        await _uow.CommitTransactionAsync();
        return true;
    }
}