namespace IMS.Domain.Entities;
// This Class As SNAPSHOT 
// Says => Warehouse(warehouseId) have (x) Product That is ID = (productId)
public class Stock : BaseEntity
{
    public Guid ProductId { get; set; }                  // Related product
    public Guid WarehouseId { get; set; }                // Warehouse location
    public decimal Quantity { get; set; }                // Physical quantity in warehouse
    public decimal ReservedQuantity { get; set; }        // Quantity booked for orders
    public decimal MinQuantityLevel { get; set; }        // Alert level per warehouse
    public decimal AvgCost { get; set; }                 // Real cost source for this warehouse
    public decimal AvailableQuantity => Quantity - ReservedQuantity; // Ready to sell
    public Product Product { get; set; }
    public Warehouse Warehouse { get; set; }
}
