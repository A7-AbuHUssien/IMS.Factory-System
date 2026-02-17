namespace IMS.Application.Modules.Sales.DTOs.Order;

public class CreateReturnedItemDto
{
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public string? Reason { get; set; }
    public string Source { get; set; }
}