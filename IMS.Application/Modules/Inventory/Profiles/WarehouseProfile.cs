using AutoMapper;
using IMS.Application.Modules.Inventory.DTOs.Warehouse;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Inventory.Profiles;

public class WarehouseProfile : Profile
{
    public  WarehouseProfile()
    {
        CreateMap<CreateWarehouseDto, Warehouse>();
        
        CreateMap<UpdateWarehouseDto, Warehouse>()
            .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        
        CreateMap<Warehouse, WarehouseDto>()
            .ForMember(dest => dest.StocksCount, opt => opt.MapFrom(src => src.Stocks.Count))
            .ForMember(dest => dest.StockTransactionsCount, opt => opt.MapFrom(src =>
                src.StockTransactions.Count));
        
    }
}