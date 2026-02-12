
using IMS.Application.Modules.Inventory.DTOs.Stock;
using IMS.Application.Modules.Inventory.Interfaces;
using IMS.Application.Modules.Inventory.UseCases;

namespace IMS.Application.Modules.Inventory.Services;
public class StockService : IStockService
{
    private readonly ReceiveStockUseCase _receive;
    private readonly TransferStockUseCase _transfer;
    private readonly AdjustStockUseCase _adjust;

    public StockService(
        ReceiveStockUseCase receive,
        TransferStockUseCase transfer,
        AdjustStockUseCase adjust)
    {
        _receive = receive;
        _transfer = transfer;
        _adjust = adjust;
    }

    public Task ReceiveAsync(ReceiveStockDto dto) => _receive.Execute(dto);

    public Task TransferAsync(TransferStockDto dto) => _transfer.Execute(dto);

    public Task AdjustAsync(AdjustStockDto dto) => _adjust.Execute(dto);
}