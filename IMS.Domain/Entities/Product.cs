using IMS.Domain.Enums;

namespace IMS.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }                     // Product display name
    public string SKU { get; set; }                      // Unique stock keeping code
    public string Description { get; set; }              // Optional product details
    public bool IsActive { get; set; }                   // Can the product be sold
    public decimal UnitPrice { get; set; }               // Default selling price (not historical)
    public UnitOfMeasure UnitOfMeasure { get; set; }     // Piece, box, kg…etc

    public decimal LastCost { get; set; }                // Cost of last received batch
    public decimal StandardCost { get; set; }            // Expected cost for comparison

    public ICollection<Stock> Stocks { get; set; }       // Stock balances per warehouse
    public ICollection<StockTransaction> StockTransactions { get; set; } // All movements
    public ICollection<SalesOrderItem> SalesOrderItems { get; set; }     // Sold lines
    public ICollection<InventoryAdjustment> InventoryAdjustments { get; set; } // Stock corrections
}
