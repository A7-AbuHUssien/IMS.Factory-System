using IMS.Domain.Entities;

namespace IMS.Application.Modules.Inventory.DomainServices;
public class StockCalculator
{
    public decimal CalculateAvg(decimal oldAVG,decimal oldQty,decimal newCost,decimal newQty)
    {
        return ((oldQty * oldAVG) + (newQty * newCost))/(oldQty + newQty);
    }
    public decimal CalculateAvg(IEnumerable<StockTransaction>  stockTransactions)
    {
       throw  new NotImplementedException();
    }
    public decimal CalculateNewBalance(decimal current, decimal change)
    {
        return current + change;
    }

    public decimal CalculateCostImpact(decimal qty, decimal unitCost)
    {
        return qty * unitCost;
    }
}
