using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Sales.DTOs.Order;
using IMS.Application.Modules.Sales.DTOs.Order.OrderItem;
using IMS.Application.Modules.Sales.Filters;
using IMS.Application.Modules.Sales.Interfaces;
using IMS.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
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
    [Authorize(Roles = AppRoles.User)]
    [HttpPost("create-draft-order")]
    public async Task<ApiResponse<OrderCreatedResult>> CreateDraftOrder(CreateOrderDto dto)
    {
        return new ApiResponse<OrderCreatedResult>(await _salesOrderService.CreateDraftOrder(dto));
    }
    [Authorize(Roles = AppRoles.User)]
    [HttpPost("add-item")]
    public async Task<ApiResponse<OrderItemDto>> AddItem(AddItemDto dto)
    {
        return new ApiResponse<OrderItemDto>(await _salesOrderService.AddItem(dto));
    }
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost("confirm/{orderId}")]
    public async Task<ApiResponse<bool>> Confirm(Guid orderId)
    {
        return new ApiResponse<bool>(await _salesOrderService.Confirm(orderId));
    }
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost("cancel/{orderId}")]
    public async Task<ApiResponse<bool>> Cancel(Guid orderId)
    {
        return new ApiResponse<bool>(await _salesOrderService.Cancel(orderId));
    }
    [Authorize(Roles = AppRoles.Admin)]
    [HttpPost("complete/{orderId}")]
    public async Task<ApiResponse<bool>> Complete(Guid orderId)
    {
        return new ApiResponse<bool>(await _salesOrderService.Complete(orderId));
    }
    
    [Authorize(Roles = AppRoles.User)]
    [HttpPost("remove-item/{orderId}/{itemId}")]
    public async Task<ApiResponse<bool>> RemoveItem(Guid orderId, Guid itemId)
    {
        return new ApiResponse<bool>(await _salesOrderService.RemoveItem(orderId, itemId));
    }
    
    [Authorize(Roles = AppRoles.User)]
    [HttpPost("submit/{orderId}")]
    public async Task<ApiResponse<bool>> Submit(Guid orderId)
    {
        return new ApiResponse<bool>(await _salesOrderService.Submit(orderId));
    }
    [Authorize(Roles =  AppRoles.User)]
    [HttpPost("return")]
    public async Task<ApiResponse<ReturnedItemDto>> Return(CreateReturnedItemDto dto)
    {
        return new ApiResponse<ReturnedItemDto>(await _salesOrderService.Return(dto));
    }
    
    [Authorize(Roles = AppRoles.User)]
    [HttpPatch("update-item-quantity/{orderId}/{itemId}/{quantity}")]
    public async Task<ApiResponse<bool>> UpdateItemQuantity(Guid orderId, Guid itemId, int quantity)
    {
        return new ApiResponse<bool>(await _salesOrderService.UpdateItemQuantity(orderId, itemId, quantity));
    }
    
    [Authorize]
    [HttpGet("{orderId}")]
    public async Task<ApiResponse<OrderDetailsDto>> GetOne(Guid orderId)
    {
        return new ApiResponse<OrderDetailsDto>(await _salesOrderService.GetOrderDetails(orderId));
    }
    [Authorize(Roles = $"{AppRoles.Manager},{AppRoles.Admin},{AppRoles.User}")]
    [HttpGet("get-orders")]
    public async Task<PaginatedApiResponse<OrderDto>> GetOrders([FromQuery] OrderFilter filter)
    {
        return await _salesOrderService.GetAll(filter);
    }
}