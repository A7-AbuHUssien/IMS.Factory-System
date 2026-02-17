namespace IMS.Application.Modules.Sales.DTOs.Order;

public class ReturnedItemDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public string? Reason { get; set; }
    public DateTime CreatedAt { get; set; }
}