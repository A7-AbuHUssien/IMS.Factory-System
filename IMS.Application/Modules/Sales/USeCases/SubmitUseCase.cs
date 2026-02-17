using IMS.Application.Common.Interfaces;
using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Sales.USeCases;

public class SubmitUseCase
{
    private readonly IUnitOfWork _uow;

    public SubmitUseCase(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<bool> Execute(Guid orderId)
    {
        SalesOrder? order =
            await _uow.SalesOrders.GetOneAsync(expression: o => o.Id == orderId, includes: [o => o.Items]);
        if (order is null) throw new BusinessException($"Order with id {orderId} not found");
        if (order.Status != SalesOrderStatus.Draft) throw new BusinessException($"Only Draft orders Can be Submitted");
        if (!order.Items.Any()) throw new BusinessException($"There's no items in order {orderId}");

        order.Status = SalesOrderStatus.Pending;
        _uow.SalesOrders.Update(order);
        await _uow.CommitAsync();
        return true;
    }
}