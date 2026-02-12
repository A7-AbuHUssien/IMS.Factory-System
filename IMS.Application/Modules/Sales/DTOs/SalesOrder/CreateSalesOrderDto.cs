using IMS.Application.Modules.Sales.DTOs.SalesOrderItem;

namespace IMS.Application.Modules.Sales.DTOs.SalesOrder;
public class CreateSalesOrderDto
{
    public Guid CustomerId { get; set; }

    public List<CreateSalesOrderItemDto> Items { get; set; }

    public decimal TaxAmount { get; set; }

    public decimal Discount { get; set; }
}
