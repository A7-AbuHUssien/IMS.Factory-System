using IMS.Domain.Exceptions;

namespace IMS.Domain.Entities;
// This Class As SNAPSHOT 
// Says => Warehouse(warehouseId) have (x) Product That is ID = (productId)
public class Stock : BaseEntity
{
    public Guid ProductId { get; set; } 
    public Guid WarehouseId { get; set; } 
    public decimal Quantity { get; set; } 
    public decimal ReservedQuantity { get; set; }
    public decimal AvgCost { get; set; }
    public decimal AvailableQuantity => Quantity - ReservedQuantity;
    public Product Product { get; set; }
    public Warehouse Warehouse { get; set; }
    
    
    public void Reserve(decimal quantity)
    {
        if (quantity <= 0) throw new BusinessException("Quantity must be positive");
        if (AvailableQuantity < quantity) 
            throw new BusinessException("Not enough available stock to reserve");

        ReservedQuantity += quantity;
    }
}
