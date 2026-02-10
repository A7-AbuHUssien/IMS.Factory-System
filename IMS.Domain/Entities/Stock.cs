namespace IMS.Domain.Entities;
public class Stock : BaseEntity
{
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }

    public decimal Quantity { get; set; }
    public decimal MinQuantityLevel { get; set; }
    public decimal ReservedQuantity { get; set; }
    
    public Product Product { get; set; }
    public Warehouse Warehouse { get; set; }

}