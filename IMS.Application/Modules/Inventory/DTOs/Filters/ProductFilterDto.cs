using IMS.Application.Common.DTOs;

namespace IMS.Application.Modules.Inventory.DTOs.Filters;

public class ProductFilterDto : PaginationParamsDto
{
    public string? Search { get; set; } = String.Empty;
}