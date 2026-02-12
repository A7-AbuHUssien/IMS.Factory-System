using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Inventory.DomainServices;
using IMS.Application.Modules.Inventory.DTOs.Stock;
using IMS.Application.Modules.Inventory.UseCases;
using IMS.Domain.Enums;
namespace IMS.Application.Modules.Inventory.UseCases;
public class ReceiveStockUseCase : BaseStockUseCase
{
    private readonly StockCalculator _calc;
    private readonly StockTransactionFactory _factory;

    public ReceiveStockUseCase(IUnitOfWork uow, StockCalculator calc, StockTransactionFactory factory) : base(uow)
    {
        _calc = calc;
        _factory = factory;
    }

    public async Task Execute(ReceiveStockDto dto)
    {
        StockGuard.EnsurePositiveQuantity(dto.Quantity);
        StockGuard.EnsureValidCost(dto.UnitCost);

        await _uow.BeginTransactionAsync();

        try
        {
            var stock = await GetOrCreateStock(dto.ProductId, dto.WarehouseId);

            stock.Quantity += dto.Quantity;
            
            var trans = _factory.CreateIn(dto.ProductId, dto.WarehouseId, dto.Quantity, dto.UnitCost,
                stock.Quantity, dto.Reference ?? "Receive",TransactionSource.ManualReceive);

            await _uow.StockTransactions.CreateAsync(trans);
            
            // Here, with the high volume of operations, the system will make a small, perhaps negligible, error.
            // A back-end service should be implemented for modification and review at the end of each week or day. 
            stock.AvgCost = _calc.CalculateAvg(stock.AvgCost, stock.Quantity, dto.UnitCost,dto.Quantity);
            _uow.Stocks.Update(stock);
            
            await _uow.CommitTransactionAsync();
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }
}