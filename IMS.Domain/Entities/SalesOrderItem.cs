namespace IMS.Domain.Entities;

public class SalesOrderItem : BaseEntity
{
    public Guid SalesOrderId { get; set; }
    public Guid ProductId { get; set; }

    public decimal Quantity { get; set; }
    public decimal Price { get; set; }
    
    public SalesOrder SalesOrder { get; set; }
    public Product Product { get; set; }

}