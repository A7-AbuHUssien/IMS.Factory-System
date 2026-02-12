using AutoMapper;
using IMS.Application.Modules.Sales.DTOs.SalesOrder;
using IMS.Application.Modules.Sales.DTOs.SalesOrderItem;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Sales.Profiles;

public class SalesOrderProfile : Profile
{
    public SalesOrderProfile()
    {
        // ---------------- CREATE ----------------

        CreateMap<CreateSalesOrderDto, SalesOrder>()
            .ForMember(dest => dest.Total,
                opt => opt.Ignore())
            .ForMember(dest => dest.OrderNumber,
                opt => opt.Ignore())
            .ForMember(dest => dest.Items,
                opt => opt.Ignore());

        CreateMap<CreateSalesOrderItemDto, SalesOrderItem>();


        // ---------------- UPDATE ----------------

        CreateMap<UpdateSalesOrderDto, SalesOrder>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));


        // ---------------- VIEW ----------------

        CreateMap<SalesOrderItem, SalesOrderItemDto>()
            .ForMember(dest => dest.ProductName,
                opt => opt.MapFrom(src => src.Product.Name))
            .ForMember(dest => dest.UnitPrice,
                opt => opt.MapFrom(src => src.Product.UnitPrice))
            .ForMember(dest => dest.LineTotal,
                opt => opt.MapFrom(src => 
                    src.Product.UnitPrice * src.Quantity));

        CreateMap<SalesOrder, SalesOrderDto>()
            .ForMember(dest => dest.CustomerName,
                opt => opt.MapFrom(src => src.Customer.Name))

            .ForMember(dest => dest.SubTotal,
                opt => opt.MapFrom(src => src.Total))

            .ForMember(dest => dest.NetTotal,
                opt => opt.MapFrom(src =>
                    src.Total + src.TaxAmount - src.Discount))

            .ForMember(dest => dest.ItemsCount,
                opt => opt.MapFrom(src => src.Items.Count));
    }
}
