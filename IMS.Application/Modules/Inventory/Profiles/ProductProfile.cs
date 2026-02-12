using AutoMapper;
using IMS.Application.Modules.Inventory.DTOs.Product;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Inventory.Profiles;

public class ProductProfile  : Profile
{
    public ProductProfile()
    {
        // Create
        CreateMap<CreateProductDto, Product>();

        // Update
        CreateMap<UpdateProductDto, Product>();

        // View
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.StockCount,
                opt => opt.MapFrom(src => src.Stocks.Count))
            .ForMember(dest => dest.TransactionsCount,
                opt => opt.MapFrom(src => src.StockTransactions.Count))
            .ForMember(dest => dest.SalesCount,
                opt => opt.MapFrom(src => src.SalesOrderItems.Count));
    }
}