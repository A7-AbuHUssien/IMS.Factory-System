using AutoMapper;
using AutoMapper.QueryableExtensions;
using IMS.Application.Common.DTOs;
using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Sales.DTOs;
using IMS.Application.Modules.Sales.DTOs.Order;
using IMS.Application.Modules.Sales.DTOs.Order.OrderItem;
using IMS.Application.Modules.Sales.Filters;
using IMS.Application.Modules.Sales.Interfaces;
using IMS.Application.Modules.Sales.USeCases;
using IMS.Domain.Exceptions;
using Microsoft.EntityFrameworkCore;


namespace IMS.Application.Modules.Sales.Services;

public class SalesOrderService : ISalesOrderService
{
    private readonly IUnitOfWork _uow;
    private readonly CreateOrderUseCase _createOrderUseCase;
    private readonly AddItemUseCase _addItemUseCase;
    private readonly ConfirmUseCase _confirm;
    private readonly CancelUseCase _cancel;
    private readonly CompleteUseCase _complete;
    private readonly RemoveItemUseCase _removeItem;
    private readonly UpdateItemQuantityUseCase _updateItemQuantity;
    private readonly IMapper _mapper;
    public SalesOrderService(
        IUnitOfWork uow,
        CreateOrderUseCase createOrderUseCase,
        AddItemUseCase addItemUseCase,
        ConfirmUseCase confirm, 
        CancelUseCase cancelUseCase, 
        CompleteUseCase completeUseCase,
        RemoveItemUseCase removeItem, 
        UpdateItemQuantityUseCase updateItemQuantity,
        IMapper mapper)
    {
        _createOrderUseCase = createOrderUseCase;
        _addItemUseCase = addItemUseCase;
        _confirm = confirm;
        _cancel = cancelUseCase;
        _complete = completeUseCase;
        _removeItem = removeItem;
        _updateItemQuantity = updateItemQuantity;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<OrderCreatedResult> CreateDraftOrder(CreateOrderDto dto)
    {
        return await _createOrderUseCase.Execute(dto);
    }

    public async Task<OrderItemDto> AddItem(AddItemDto dto)
    {
        return await _addItemUseCase.Execute(dto);
    }

    public async Task<bool> Confirm(Guid orderId)
    {
        return await _confirm.Execute(orderId);
    }

    public async Task<bool> Cancel(Guid orderId)
    {
        return await _cancel.Execute(orderId);
    }

    public async Task<bool> Complete(Guid orderId)
    {
        return await _complete.Execute(orderId);
    }

    public async Task<bool> RemoveItem(Guid orderId, Guid itemId)
    {
        return await _removeItem.Execute(orderId, itemId);
    }

    public async Task<bool> UpdateItemQuantity(Guid orderId, Guid itemId, int quantity)
    {
       return await _updateItemQuantity.Execute(orderId, itemId, quantity);
    }

    public async Task<OrderDetailsDto> GetOrderDetails(Guid orderId)
    {
        var order = await _uow.SalesOrders.Query()
            .Where(o => o.Id == orderId)
            .Include(o => o.Items.Where(i=>i.IsDeleted == false))
            .ProjectTo<OrderDetailsDto>(_mapper.ConfigurationProvider)
            .FirstOrDefaultAsync();
        if (order == null) throw new BusinessException("Order not found");
        return order;
    }
    public async Task<PaginatedApiResponse<OrderDto>> GetAll(OrderFilter filter)
    {
        var query = _uow.SalesOrders.Query().AsNoTracking();

        if (!string.IsNullOrWhiteSpace(filter.Search))
        {
            query = query.Where(o =>
                o.OrderNumber.Contains(filter.Search) ||
                o.Customer.Name.Contains(filter.Search));
        }

        var totalCount = await query.CountAsync();

        var items = await query
            .Skip((filter.PageNumber - 1) * filter.PageSize)
            .Take(filter.PageSize)
            .ProjectTo<OrderDto>(_mapper.ConfigurationProvider)
            .ToListAsync();
        
        return new PaginatedApiResponse<OrderDto>(items,filter.PageNumber,filter.PageSize,totalCount);
    }
    
}