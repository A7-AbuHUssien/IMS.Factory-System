using IMS.Domain.Enums;

namespace IMS.Application.Modules.Sales.DTOs.SalesOrder;
public class UpdateSalesOrderDto
{
    public int Id { get; set; }

    public SalesOrderStatus? Status { get; set; }

    public decimal? TaxAmount { get; set; }

    public decimal? Discount { get; set; }
}