using IMS.Domain.Enums;

namespace IMS.Application.Modules.Inventory.DTOs.Product;

public class UpdateProductDto
{
    public Guid Id { get; set; }

    public string Name { get; set; }
    public string SKU { get; set; }
    public string Description { get; set; }

    public int? ReorderLevel { get; set; }
    public decimal? UnitPrice { get; set; }
    public UnitOfMeasure? UnitOfMeasure { get; set; }
    public bool? IsActive { get; set; }
}