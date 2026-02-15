using IMS.Application.Common.DTOs;
using IMS.Application.Modules.Inventory.DTOs.Filters;
using IMS.Application.Modules.Inventory.DTOs.Stock;
using IMS.Application.Modules.Inventory.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IMS.API.Controllers.Inventory;
[Route("api/inventory/[controller]")]
[ApiController]
public class StocksController : ControllerBase
{
    private readonly IStockService _stockService;

    public StocksController(IStockService stockService)
    {
        _stockService = stockService;
    }

    [HttpGet]
    public async Task<PaginatedApiResponse<StockDto>> ListStocks([FromQuery] StockFilterDto filter)
    {
        return await _stockService.GetStocksAsync(filter);
    }
    
    [HttpGet("{warehouseId:guid}/{productId:guid}")]
    public async Task<ApiResponse<StockDto?>> GetStock(Guid stockId)
    {
        return new ApiResponse<StockDto?>(await _stockService.GetSingleStockAsync(stockId));
    }
    
    [HttpPost("receive")]
    public async Task<IActionResult> Receive(ReceiveStockDto dto)
    {
        await _stockService.ReceiveAsync(dto);
        return Created();
    }
    
    [HttpPost("transfer")]
    public async Task<IActionResult> Transfer(TransferStockDto dto)
    {
        await _stockService.TransferAsync(dto);
        return Created();
    }

    [HttpPost("adjust")]
    public async Task<IActionResult> Adjust(AdjustStockDto dto)
    {
        await _stockService.AdjustAsync(dto);
        return Created();
    }
}