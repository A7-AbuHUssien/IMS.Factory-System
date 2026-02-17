using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Sales.DTOs.Order;
using IMS.Application.Modules.Sales.DTOs.Order.OrderItem;
using IMS.Application.Modules.Sales.Filters;
using IMS.Application.Modules.Sales.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers.Sales;

[Route("api/sales/[controller]")]
[ApiController]
public class OrdersController : ControllerBase
{
    private readonly ISalesOrderService _salesOrderService;

    public OrdersController(ISalesOrderService salesOrderService)
    {
        _salesOrderService = salesOrderService;
    }

    [HttpPost("create-draft-order")]
    public async Task<ApiResponse<OrderCreatedResult>> CreateDraftOrder(CreateOrderDto dto)
    {
        return new ApiResponse<OrderCreatedResult>(await _salesOrderService.CreateDraftOrder(dto));
    }

    [HttpPost("add-item")]
    public async Task<ApiResponse<OrderItemDto>> AddItem(AddItemDto dto)
    {
        return new ApiResponse<OrderItemDto>(await _salesOrderService.AddItem(dto));
    }

    [HttpPost("confirm/{orderId}")]
    public async Task<ApiResponse<bool>> Confirm(Guid orderId)
    {
        return new ApiResponse<bool>(await _salesOrderService.Confirm(orderId));
    }

    [HttpPost("cancel/{orderId}")]
    public async Task<ApiResponse<bool>> Cancel(Guid orderId)
    {
        return new ApiResponse<bool>(await _salesOrderService.Cancel(orderId));
    }

    [HttpPost("complete/{orderId}")]
    public async Task<ApiResponse<bool>> Complete(Guid orderId)
    {
        return new ApiResponse<bool>(await _salesOrderService.Complete(orderId));
    }

    [HttpPost("remove-item/{orderId}/{itemId}")]
    public async Task<ApiResponse<bool>> RemoveItem(Guid orderId, Guid itemId)
    {
        return new ApiResponse<bool>(await _salesOrderService.RemoveItem(orderId, itemId));
    }


    [HttpPost("submit/{orderId}")]
    public async Task<ApiResponse<bool>> Submit(Guid orderId)
    {
        return new ApiResponse<bool>(await _salesOrderService.Submit(orderId));
    }

    [HttpPost("return")]
    public async Task<ApiResponse<ReturnedItemDto>> Return(CreateReturnedItemDto dto)
    {
        return new ApiResponse<ReturnedItemDto>(await _salesOrderService.Return(dto));
    }

    [HttpPatch("update-item-quantity/{orderId}/{itemId}/{quantity}")]
    public async Task<ApiResponse<bool>> UpdateItemQuantity(Guid orderId, Guid itemId, int quantity)
    {
        return new ApiResponse<bool>(await _salesOrderService.UpdateItemQuantity(orderId, itemId, quantity));
    }

    [HttpGet("{orderId}")]
    public async Task<ApiResponse<OrderDetailsDto>> GetOne(Guid orderId)
    {
        return new ApiResponse<OrderDetailsDto>(await _salesOrderService.GetOrderDetails(orderId));
    }

    [HttpGet("get-orders")]
    public async Task<PaginatedApiResponse<OrderDto>> GetOrders([FromQuery] OrderFilter filter)
    {
        return await _salesOrderService.GetAll(filter);
    }
}