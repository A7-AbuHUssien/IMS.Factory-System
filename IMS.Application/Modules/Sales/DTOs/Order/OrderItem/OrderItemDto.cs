namespace IMS.Application.Modules.Sales.DTOs.Order.OrderItem;

public class OrderItemDto
{
    public Guid ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal UnitPrice { get; set; }

    public decimal Quantity { get; set; }

    public decimal LineTotal { get; set; }
}