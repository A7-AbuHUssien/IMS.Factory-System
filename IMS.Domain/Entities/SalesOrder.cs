using IMS.Domain.Enums;

namespace IMS.Domain.Entities;

public class SalesOrder : BaseEntity
{
    public Guid CustomerId { get; set; }
    public Guid WarehouseId { get; set; }
    public SalesOrderStatus Status { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    
    public decimal Total { get; set; }
    public decimal TaxAmount { get; set; }
    public decimal Discount { get; set; }
    
    public decimal TotalCost { get; set; }
    public decimal TotalProfit { get; set; }

    public Customer Customer { get; set; }
    public Warehouse Warehouse { get; set; }
    public ICollection<SalesOrderItem> Items { get; set; }
}
