namespace IMS.Application.Modules.Inventory.DTOs.Stock;

public class StockDto
{
    public Guid Id { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    
    public string SKU { get; set; }
    public string ProductName { get; set; }
    public string WarehouseName { get; set; }
    public decimal Quantity { get; set; }     
    public decimal ReservedQuantity { get; set; }

    public decimal AvailableQuantity => Quantity - ReservedQuantity;
    
}