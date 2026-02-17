namespace IMS.Domain.Entities;

public class ReturnedItem
{
    public Guid Id { get; set; }

    public Guid ProductId { get; set; }

    public decimal Quantity { get; set; }

    public string? Reason { get; set; }

    public DateTime CreatedAt { get; set; }

    public Guid? CreatedBy { get; set; }

    public string Source { get; set; } // Factory / Customer / Manual
}