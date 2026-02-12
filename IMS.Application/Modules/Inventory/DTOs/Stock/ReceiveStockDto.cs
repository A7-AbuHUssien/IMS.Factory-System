namespace IMS.Application.Modules.Inventory.DTOs.Stock;

public class ReceiveStockDto
{
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }

    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }

    public Guid? UserId { get; set; }
    public string? Reference { get; set; }
}
