using IMS.Domain.Enums;

namespace IMS.Domain.Entities;

public class ReservationRequests : BaseEntity
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public Guid WarehouseId { get; set; }
    public decimal Quantity { get; set; }
    public ReservationStatus Status { get; set; }
    
    public SalesOrder Order { get; set; }
    public Product Product { get; set; }
}