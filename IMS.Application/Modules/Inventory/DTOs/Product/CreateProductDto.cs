using IMS.Domain.Enums;

namespace IMS.Application.Modules.Inventory.DTOs.Product;

public class CreateProductDto
{
    public string Name { get; set; }
    public string SKU { get; set; }
    public string Description { get; set; }
    public bool IsActive { get; set; }
    public decimal UnitCost { get; set; }
    public decimal UnitPrice { get; set; }
    public UnitOfMeasure UnitOfMeasure { get; set; }
}
