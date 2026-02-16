using IMS.Domain.Enums;

namespace IMS.Domain.Entities;

public class SalesOrder : BaseEntity
{
    public Guid CustomerId { get; set; }
    public SalesOrderStatus Status { get; set; }
    public string OrderNumber { get; set; }
    public DateTime OrderDate { get; set; }
    public decimal TotalPrice { get; set; }
    public decimal TotalCost { get; set; }
    public decimal TotalProfit => TotalPrice - TotalCost;

    public Customer Customer { get; set; }
    public ICollection<SalesOrderItem> Items { get; set; } =  new List<SalesOrderItem>();
    
    public void RecalculateTotals()
    {
        TotalCost = Items.Sum(i => i.UnitCostAtSale * i.Quantity);
        TotalPrice = Items.Sum(i => i.UnitPriceAtSale * i.Quantity);
    }
}
