using IMS.Domain.Enums;

namespace IMS.Domain.Entities;

public class Product : BaseEntity
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public string Description { get; set; }

    public int ReorderLevel { get; set; }
    public bool IsActive { get; set; }
    public decimal UnitPrice { get; set; }
    public UnitOfMeasure UnitOfMeasure { get; set; }
    public ICollection<Stock> Stocks { get; set; }
    public ICollection<StockTransaction> StockTransactions { get; set; }
    public ICollection<SalesOrderItem> SalesOrderItems { get; set; }
}
