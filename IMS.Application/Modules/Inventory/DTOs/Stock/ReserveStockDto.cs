namespace IMS.Application.Modules.Inventory.DTOs.Stock;

public class ReserveStockDto
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public decimal Quantity { get; set; }
    public DateTime Date { get; set; }
}