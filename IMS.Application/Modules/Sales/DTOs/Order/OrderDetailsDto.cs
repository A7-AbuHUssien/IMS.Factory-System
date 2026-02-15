using IMS.Application.Modules.Sales.DTOs.Order.OrderItem;
using IMS.Domain.Enums;

namespace IMS.Application.Modules.Sales.DTOs.Order;

public class OrderDetailsDto
{
    public Guid Id { get; set; }

    public string OrderNumber { get; set; }

    public string CustomerName { get; set; }

    public SalesOrderStatus Status { get; set; }

    public decimal Total { get; set; }
    
    public int ItemsCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<OrderItemDto> Items { get; set; }
}