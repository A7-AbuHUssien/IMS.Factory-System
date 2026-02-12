using IMS.Application.Modules.Inventory.DTOs.Warehouse;

namespace IMS.Application.Modules.Inventory.Interfaces;

public interface IWarehouseService
{
    Task<WarehouseDto> CreateWarehouse(CreateWarehouseDto warehouseDto);
    Task<WarehouseDto> UpdateWarehouse(UpdateWarehouseDto dto);
    Task<WarehouseDto> GetWarehouseById(Guid id);
    Task<IEnumerable<WarehouseDto>> GetWarehouses();
    Task<bool> DeleteWarehouse(Guid id);
}