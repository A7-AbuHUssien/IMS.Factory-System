using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Inventory.DTOs.Stock;
using IMS.Application.Modules.Inventory.UseCases;
using IMS.Application.Modules.Sales.DTOs.Order;
using IMS.Application.Modules.Sales.DTOs.Order.OrderItem;
using IMS.Domain.DomainServices;
using IMS.Domain.Entities;
using IMS.Domain.Enums;
using IMS.Domain.Exceptions;

namespace IMS.Application.Modules.Sales.USeCases;

public class AddItemUseCase
{
    private readonly IUnitOfWork _uow;

    public AddItemUseCase(IUnitOfWork uow)
    {
        _uow = uow;
    }

    public async Task<OrderItemDto> Execute(AddItemDto dto)
    {
        var order = await _uow.SalesOrders.GetOneAsync(e => e.Id == dto.OrderId);
        if (order == null)
            throw new BusinessException("Order not found");

        if (order.Status != SalesOrderStatus.Pending)
            throw new BusinessException("Order locked");

        var product = await _uow.Products.GetOneAsync(e => e.Id == dto.ProductId);
        if (product == null)
            throw new BusinessException("Product not found");

        StockGuard.EnsureNoNegative(dto.Quantity);
        var existed =
            await _uow.SalesOrderItems.GetOneAsync(e => e.ProductId == product.Id && e.SalesOrderId == order.Id);
        if (existed != null)
        {
            existed.Quantity += dto.Quantity;
            existed.UnitCostAtSale = product.UnitPrice;
            existed.UnitPriceAtSale = product.UnitPrice; 
            _uow.SalesOrderItems.Update(existed);
        }
        else
        {
            existed = await _uow.SalesOrderItems.CreateAsync(new SalesOrderItem()
            {
                ProductId = product.Id,
                SalesOrderId = dto.OrderId,
                Quantity = dto.Quantity,
                UnitPriceAtSale = product.UnitPrice,
                UnitCostAtSale = product.AVGUnitCost
            });
        }
       
        order.RecalculateTotals();
        _uow.SalesOrders.Update(order);
        
        
        await _uow.CommitAsync();
        return new OrderItemDto()
        {
            ProductId = product.Id,
            ProductName = product.Name,
            Quantity = existed.Quantity,
            UnitPrice = product.UnitPrice,
            LineTotal = existed.Quantity * product.UnitPrice
        };
    }
}