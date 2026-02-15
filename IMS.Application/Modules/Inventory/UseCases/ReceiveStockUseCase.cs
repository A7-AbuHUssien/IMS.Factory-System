using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Inventory.DTOs.Stock;
using IMS.Domain.DomainServices;
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
        var stock = await GetOrCreateStock(dto.ProductId, dto.WarehouseId);
        bool wasExist = stock.Quantity != 0;
       await _uow.BeginTransactionAsync();
        try
        {
            stock.Quantity += dto.Quantity;
            // Minor drifts occur under high load; reconciled via background service
            if(wasExist)
                stock.AvgCost = _calc.CalculateAvg(stock.AvgCost, stock.Quantity, dto.UnitCost,dto.Quantity);
            else
                stock.AvgCost = dto.UnitCost;
            var trans = _factory.CreateIn(dto.ProductId, dto.WarehouseId, dto.Quantity, dto.UnitCost,
                stock.Quantity, dto.Reference ?? "Receive",TransactionSource.ManualReceive);
            await _uow.StockTransactions.CreateAsync(trans);
            if (wasExist)
                _uow.Stocks.Update(stock);
            await _uow.CommitAsync();
            await _uow.CommitTransactionAsync();
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }
}