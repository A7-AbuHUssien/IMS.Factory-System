using IMS.Application.Modules.Inventory.DTOs.Stock;

namespace IMS.Application.Modules.Inventory.Interfaces;

public interface IStockService
{
    Task ReceiveAsync(ReceiveStockDto dto);
    Task TransferAsync(TransferStockDto dto);
    Task AdjustAsync(AdjustStockDto dto);
}