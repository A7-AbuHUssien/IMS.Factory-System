using IMS.Application.Common.DTOs;

namespace IMS.Application.Modules.Sales.Filters;

public class OrderFilter : PaginationParamsDto
{
    public string? Search { get; set; }
}