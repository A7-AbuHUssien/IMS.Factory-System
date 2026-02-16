using AutoMapper;
using IMS.Application.Modules.Inventory.DTOs.Product;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Inventory.Profiles;

public class ProductProfile : Profile
{
    public ProductProfile()
    {
        // Create
        CreateMap<CreateProductDto, Product>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest=>dest.AVGUnitCost, opt=>opt.MapFrom(src=>src.UnitCost))
            .ForMember(dest => dest.UnitOfMeasure, opt => opt.MapFrom(src => src.UnitOfMeasure));
        // Update
        CreateMap<UpdateProductDto, Product>();

        // View
        CreateMap<Product, ProductDto>()
            .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
            .ForMember(dest => dest.SKU, opt => opt.MapFrom(src => src.SKU))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description))
            .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.IsActive))
            .ForMember(dest => dest.UnitPrice, opt => opt.MapFrom(src => src.UnitPrice))
            .ForMember(dest => dest.UnitOfMeasure, opt => opt.MapFrom(src => src.UnitOfMeasure))
            .ForMember(dest => dest.SalesCount, opt => opt.MapFrom(src => src.SalesOrderItems.Count()));
    }
}