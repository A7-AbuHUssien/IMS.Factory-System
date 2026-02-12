namespace IMS.Application.Modules.Sales.DTOs.SalesOrderItem;

public class SalesOrderItemDto
{
    public int ProductId { get; set; }

    public string ProductName { get; set; }

    public decimal UnitPrice { get; set; }

    public int Quantity { get; set; }

    public decimal LineTotal { get; set; }
}