using IMS.Application.Common.Interfaces;
using IMS.Application.Modules.Inventory.DomainServices;
using IMS.Application.Modules.Inventory.DTOs.Stock;
using IMS.Domain.Enums;

namespace IMS.Application.Modules.Inventory.UseCases;

public class TransferStockUseCase : BaseStockUseCase
{
    private readonly StockTransactionFactory _factory;
    private readonly StockCalculator _calc;

    public TransferStockUseCase(IUnitOfWork uow, StockTransactionFactory factory, StockCalculator calc) : base(uow)
    {
        _factory = factory;
        _calc = calc;
    }

    public async Task Execute(TransferStockDto dto)
    {
        StockGuard.EnsurePositiveQuantity(dto.Quantity);


        await _uow.BeginTransactionAsync();
        try
        {
            var from = await GetStock(dto.ProductId, dto.SourceWarehouseId);
            var to = await GetOrCreateStock(dto.ProductId, dto.DestinationWarehouseId);
            bool isNewStock = (to.AvgCost == 0 && to.Quantity == 0);
            StockGuard.EnsureEnoughStock(from.Quantity, dto.Quantity);
            to.AvgCost = _calc.CalculateAvg(to.AvgCost, to.Quantity, from.AvgCost, dto.Quantity);
            from.Quantity -= dto.Quantity;
            to.Quantity += dto.Quantity;

            var outTrans = _factory.CreateOut(dto.ProductId, dto.SourceWarehouseId, dto.Quantity, from.AvgCost, 0,
                from.Quantity,
                "Transfer-Out", TransactionSource.Transfer);
            var inTrans = _factory.CreateIn(dto.ProductId, dto.DestinationWarehouseId, dto.Quantity, from.AvgCost,
                to.Quantity,
                "Transfer-In", TransactionSource.Transfer);

            await _uow.StockTransactions.CreateAsync(outTrans);
            await _uow.StockTransactions.CreateAsync(inTrans);

            _uow.Stocks.Update(from);
            if (!isNewStock)
                _uow.Stocks.Update(to);

            await _uow.CommitTransactionAsync();
        }
        catch
        {
            await _uow.RollbackTransactionAsync();
            throw;
        }
    }
}