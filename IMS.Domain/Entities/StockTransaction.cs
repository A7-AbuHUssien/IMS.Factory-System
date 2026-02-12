using IMS.Domain.Enums;

namespace IMS.Domain.Entities;
public class StockTransaction : BaseEntity
{
    public Guid ProductId { get; set; }                  // Product affected
    public Guid WarehouseId { get; set; }                // Warehouse affected

    public StockTransactionType Type { get; set; }       // Sale, Receive, Transfer, etc
    public TransactionSource Source { get; set; }        // Business origin of movement

    public decimal Quantity { get; set; }                // Moved quantity
    
    public decimal UnitCost { get; set; }                // Cost at movement time
    public decimal UnitPrice { get; set; }               // Selling price if sale

    public decimal TotalCost => Quantity * UnitCost;             // Quantity * UnitCost
    public DateTime TransactionDate { get; set; }

    public decimal BalanceAfter { get; set; }            // Stock level after this transaction

    public Guid? UserId { get; set; }                    // Who performed the action
    public Guid? ReferenceId { get; set; }               // Link to order/adjustment
    public string ReferenceType { get; set; }
    public Product Product { get; set; }
    public Warehouse Warehouse { get; set; }
    public User User { get; set; }
}
