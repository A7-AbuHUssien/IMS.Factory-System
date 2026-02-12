namespace IMS.Application.Modules.Inventory.DTOs.Stock;

public class TransferStockDto
{
    public Guid ProductId { get; set; }
    public Guid SourceWarehouseId { get; set; }
    public Guid DestinationWarehouseId { get; set; }
    public decimal Quantity { get; set; }
    public string Notes { get; set; }
}