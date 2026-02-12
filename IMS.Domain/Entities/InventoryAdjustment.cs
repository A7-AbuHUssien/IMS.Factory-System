namespace IMS.Domain.Entities;
// UPDATE STOCK AFTER ADJUSTMENT
public class InventoryAdjustment : BaseEntity
{
    public Guid ProductId { get; set; }                  // Adjusted product
    public Guid WarehouseId { get; set; }                // Warehouse of adjustment
    
    public DateTime AdjustmentDate { get; set; }
    
    public decimal QuantityBefore { get; set; }          // Stock before change
    public decimal QuantityAdjusted { get; set; }        // + or – difference
    public decimal QuantityAfter { get; set; }           // Final quantity

    public decimal CostImpact { get; set; }              // Financial effect of adjustment

    public string Reason { get; set; }                   // Damage, count error, etc

    public Guid? AdjustedByUserId { get; set; }          // Responsible employee

    public Product Product { get; set; }
    public Warehouse Warehouse { get; set; }
    public User AdjustedByUser { get; set; }
}