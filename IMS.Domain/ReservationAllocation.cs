namespace IMS.Domain;

public class ReservationAllocation
{
    public Guid WarehouseId { get; set; }
    public decimal Quantity { get; set; }
    public decimal UnitCost { get; set; }

    public ReservationAllocation(Guid warehouseId, decimal quantity, decimal unitCost)
    {
        WarehouseId = warehouseId;
        Quantity = quantity;
        UnitCost = unitCost;
    }

    public ReservationAllocation()
    {
        
    }
}