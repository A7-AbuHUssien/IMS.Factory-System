using IMS.Application.Common.DTOs;

namespace IMS.Application.Modules.Sales.Filters;

public class CustomerFilter : PaginationParamsDto
{
    public string? Search { get; set; } = String.Empty;
}