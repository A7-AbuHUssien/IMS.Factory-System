namespace IMS.Application.Modules.Inventory.DTOs.Stock;

public class AdjustStockDto
{
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public decimal ActualQuantity { get; set; }
    public string Reason { get; set; }
}