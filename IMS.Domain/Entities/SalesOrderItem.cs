namespace IMS.Domain.Entities;

public class SalesOrderItem : BaseEntity
{
    public Guid SalesOrderId { get; set; }               // Parent order
    public Guid ProductId { get; set; }                  // Sold product
    public Guid WarehouseId { get; set; }

    public decimal Quantity { get; set; }                // Sold quantity
    public decimal UnitPriceAtSale { get; set; }         // Price snapshot at sale time
    public decimal UnitCostAtSale { get; set; }          // Cost snapshot at sale time
    public decimal LineProfit => (UnitPriceAtSale - UnitCostAtSale) * Quantity;    // (Price - Cost) * Qty
   
    public SalesOrder SalesOrder { get; set; }
    public Product Product { get; set; }
    public Warehouse Warehouse { get; set; }
}