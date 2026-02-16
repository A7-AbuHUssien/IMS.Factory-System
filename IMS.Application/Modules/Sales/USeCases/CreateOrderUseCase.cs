using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Sales.DTOs.Order;
using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Sales.USeCases;

public class CreateOrderUseCase
{
    private readonly IUnitOfWork _uow;

    public CreateOrderUseCase(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<OrderCreatedResult> Execute(CreateOrderDto dto)
    {
        if (!await _uow.Customers.Any(c => c.Id == dto.CustomerId))
            throw new BusinessException("Customer not found");

        var order = new SalesOrder
        {
            CustomerId = dto.CustomerId,
            Status = SalesOrderStatus.Pending,
            OrderDate = DateTime.UtcNow,
        };

        await _uow.SalesOrders.CreateAsync(order);
        await _uow.CommitAsync();

        return new OrderCreatedResult
        {
            OrderId = order.Id,
            OrderNumber = order.OrderNumber
        };
    }
}
