using IMS.Domain.Entities;

namespace IMS.Domain.DomainServices;
public class StockCalculator
{
    public decimal CalculateAvg(decimal oldAVG,decimal oldQty,decimal newCost,decimal newQty)
    {
        return ((oldQty * oldAVG) + (newQty * newCost))/(oldQty + newQty);
    }
}
