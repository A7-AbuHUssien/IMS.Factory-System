namespace IMS.Domain.Entities;

public class Warehouse : BaseEntity
{
    public string Name { get; set; }
    public string Location { get; set; }
    public ICollection<Stock> Stocks { get; set; }
    public ICollection<StockTransaction> StockTransactions { get; set; }
    
}
