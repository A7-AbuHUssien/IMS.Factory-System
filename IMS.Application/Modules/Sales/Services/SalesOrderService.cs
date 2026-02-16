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
    private readonly CreateOrderUseCase _createOrder;
    private readonly AddItemUseCase _addItem;
    private readonly ConfirmUseCase _confirm;
    private readonly CancelUseCase _cancel;
    private readonly CompleteUseCase _complete;
    private readonly RemoveItemUseCase _removeItem;
    private readonly UpdateItemQuantityUseCase _updateItem;
    private readonly IMapper _mapper;

    public SalesOrderService( CreateOrderUseCase createOrderUseCase, AddItemUseCase addItemUseCase, 
        ConfirmUseCase confirm, CancelUseCase cancelUseCase, CompleteUseCase completeUseCase,
        RemoveItemUseCase removeItem, UpdateItemQuantityUseCase updateItemQuantity,IUnitOfWork uow, IMapper mapper)
    {
        _createOrder = createOrderUseCase;
        _addItem = addItemUseCase;
        _confirm = confirm;
        _cancel = cancelUseCase;
        _complete = completeUseCase;
        _removeItem = removeItem;
        _updateItem = updateItemQuantity;
        _uow = uow;
        _mapper = mapper;
    }

    public async Task<OrderCreatedResult> CreateDraftOrder(CreateOrderDto dto) => await _createOrder.Execute(dto);
    public async Task<OrderItemDto> AddItem(AddItemDto dto) => await _addItem.Execute(dto);
    public async Task<bool> Confirm(Guid orderId) => await _confirm.Execute(orderId);
    public async Task<bool> Cancel(Guid orderId) => await _cancel.Execute(orderId);
    public async Task<bool> Complete(Guid orderId) => await _complete.Execute(orderId);
    public async Task<bool> RemoveItem(Guid orderId, Guid itemId) => await _removeItem.Execute(orderId, itemId);
    public async Task<bool> UpdateItemQuantity(Guid orderId, Guid itemId, int quantity) =>
        await _updateItem.Execute(orderId, itemId, quantity);


    public async Task<OrderDetailsDto> GetOrderDetails(Guid orderId)
    {
        var order = await _uow.SalesOrders.Query()
            .Where(o => o.Id == orderId)
            .Include(o => o.Items.Where(i => i.IsDeleted == false))
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

        return new PaginatedApiResponse<OrderDto>(items, filter.PageNumber, filter.PageSize, totalCount);
    }
}