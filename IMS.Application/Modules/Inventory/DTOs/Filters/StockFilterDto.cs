using IMS.Application.Common.DTOs;

namespace IMS.Application.Modules.Inventory.DTOs.Filters;

public class StockFilterDto : PaginationParamsDto
{
    public string? Search { get; set; } = String.Empty;
    public bool? LowStockOnly { get; set; } = false;
}