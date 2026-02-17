using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Sales.DTOs.Order;
using IMS.Application.Modules.Sales.DTOs.Order.OrderItem;
using IMS.Application.Modules.Sales.Filters;

namespace IMS.Application.Modules.Sales.Interfaces;

public interface ISalesOrderService
{
    Task<OrderCreatedResult> CreateDraftOrder(CreateOrderDto dto);
    Task<OrderItemDto> AddItem(AddItemDto dto);
    Task<bool> Confirm(Guid orderId);
    Task<bool> Cancel(Guid orderId);
    Task<bool> Complete(Guid orderItemId);
    Task<bool> RemoveItem(Guid orderId, Guid itemId);
    Task<bool> UpdateItemQuantity(Guid orderId, Guid itemId, int quantity);
    Task<bool> Submit(Guid orderId);
    Task<ReturnedItemDto> Return(CreateReturnedItemDto dto);
    Task<OrderDetailsDto> GetOrderDetails(Guid orderId);
    Task<PaginatedApiResponse<OrderDto>> GetAll(OrderFilter filter);
}