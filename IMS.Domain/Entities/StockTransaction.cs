using IMS.Domain.Enums;

namespace IMS.Domain.Entities;
public class StockTransaction : BaseEntity
{
    public Guid ProductId { get; set; } 
    public Guid WarehouseId { get; set; } 

    public StockTransactionType Type { get; set; }
    public TransactionSource Source { get; set; }
    public decimal Quantity { get; set; } 
    public decimal UnitCost { get; set; }

    public decimal TotalCost => Quantity * UnitCost;
    public DateTime TransactionDate { get; set; }

    public decimal BalanceAfter { get; set; }

    public Guid? UserId { get; set; }
    public Guid? ReferenceId { get; set; }
    public string ReferenceType { get; set; }
    public Product Product { get; set; }
    public Warehouse Warehouse { get; set; }
}
