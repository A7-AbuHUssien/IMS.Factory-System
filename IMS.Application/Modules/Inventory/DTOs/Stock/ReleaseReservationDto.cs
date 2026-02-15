namespace IMS.Application.Modules.Inventory.DTOs.Stock;

public class ReleaseReservationDto
{
    public Guid OrderId { get; set; }
    public Guid ProductId { get; set; }
    public string? Reason { get; set; }
}