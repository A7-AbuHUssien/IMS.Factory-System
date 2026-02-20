using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Inventory.DTOs.Warehouse;
using IMS.Application.Modules.Inventory.Interfaces;
using IMS.Domain.Constant;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers.Inventory;

[Route("api/inventory/[controller]")]
[Authorize(Roles = AppRoles.Admin)]
[ApiController]
public class WarehousesController : ControllerBase
{
    private readonly IWarehouseService _warehouseService;

    public WarehousesController(IWarehouseService warehouseService)
    {
        _warehouseService = warehouseService;
    }

    [HttpPost]
    public async Task<ApiResponse<WarehouseDto>> CreateWarehouse([FromBody] CreateWarehouseDto warehouseDto)
    {
        return new ApiResponse<WarehouseDto>(await _warehouseService.CreateWarehouse(warehouseDto),"Created");
    }

    [HttpPut]
    public async Task<ApiResponse<WarehouseDto>> UpdateWarehouse([FromBody] UpdateWarehouseDto warehouseDto)
    {
        return new ApiResponse<WarehouseDto>(await _warehouseService.UpdateWarehouse(warehouseDto),"Updated");
    }

    [HttpGet]
    public async Task<ApiResponse<IEnumerable<WarehouseDto>>> GetWarehouses()
    {
        return new ApiResponse<IEnumerable<WarehouseDto>>(await _warehouseService.GetWarehouses());
    }
    [HttpGet("{id}")]
    public async Task<ApiResponse<WarehouseDto>> GetWarehouseById(Guid id)
    {
        return new ApiResponse<WarehouseDto>(await _warehouseService.GetWarehouseById(id));
    }

    [HttpDelete("{id}")]
    public async Task<ApiResponse<bool>> DeleteWarehouse(Guid id)
    {
       await _warehouseService.DeleteWarehouse(id);
        return new ApiResponse<bool>(true);
    }
    
}