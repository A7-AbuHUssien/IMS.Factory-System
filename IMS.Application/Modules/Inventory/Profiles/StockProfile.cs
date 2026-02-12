using AutoMapper;
using IMS.Application.Modules.Inventory.DTOs.Stock;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Inventory.Profiles;

public class StockProfile : Profile
{
    public StockProfile()
    {
        // READ: Stock -> DTO
        CreateMap<Stock, StockDto>()
            .ForMember(dest => dest.ProductName, opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.Product.SKU))
            .ForMember(dest => dest.WarehouseName, opt => opt.MapFrom(src => src.Warehouse.Name))
            .ReverseMap();
        
    }
}