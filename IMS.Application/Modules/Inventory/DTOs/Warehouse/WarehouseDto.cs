namespace IMS.Application.Modules.Inventory.DTOs.Warehouse;

public class WarehouseDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Location { get; set; }
    public int StocksCount { get; set; }
    public int StockTransactionsCount { get; set; }
}