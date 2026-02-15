namespace IMS.Application.Modules.Sales.DTOs.Order;

public class AddItemDto
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
}