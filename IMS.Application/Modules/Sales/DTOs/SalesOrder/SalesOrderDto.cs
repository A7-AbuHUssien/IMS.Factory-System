using IMS.Application.Modules.Sales.DTOs.SalesOrderItem;
using IMS.Domain.Enums;

namespace IMS.Application.Modules.Sales.DTOs.SalesOrder;
public class SalesOrderDto
{
    public int Id { get; set; }

    public string OrderNumber { get; set; }

    public string CustomerName { get; set; }

    public SalesOrderStatus Status { get; set; }

    public decimal SubTotal { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal Discount { get; set; }

    public decimal NetTotal { get; set; }

    public int ItemsCount { get; set; }

    public DateTime CreatedAt { get; set; }

    public List<SalesOrderItemDto> Items { get; set; }
}
