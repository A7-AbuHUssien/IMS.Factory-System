using AutoMapper;
using IMS.Application.Modules.Sales.DTOs.Order;
using IMS.Application.Modules.Sales.DTOs.Order.OrderItem;
using IMS.Domain.Entities;

namespace IMS.Application.Modules.Sales.Profiles;

public class SalesOrderProfile : Profile
{
    public SalesOrderProfile()
    {
        // ---------------- CREATE ----------------

        CreateMap<CreateOrderDto, SalesOrder>()
            .ForMember(dest => dest.TotalPrice,
                opt => opt.Ignore())
            .ForMember(dest => dest.OrderNumber,
                opt => opt.Ignore())
            .ForMember(dest => dest.Items,
                opt => opt.Ignore());

        CreateMap<CreateOrderItemDto, SalesOrderItem>();


        // ---------------- UPDATE ----------------

        CreateMap<UpdateOrderDto, SalesOrder>()
            .ForAllMembers(opt =>
                opt.Condition((src, dest, srcMember) => srcMember != null));


        // ---------------- VIEW ----------------

        CreateMap<SalesOrderItem, OrderItemDto>()
            .ForMember(d => d.ProductName,
                opt => opt.MapFrom(s => s.Product.Name))

            .ForMember(d => d.UnitPrice,
                opt => opt.MapFrom(s => s.UnitPriceAtSale))

            .ForMember(d => d.LineTotal,
                opt => opt.MapFrom(s => s.UnitPriceAtSale * s.Quantity));


        CreateMap<SalesOrder, OrderDto>()
            .ForMember(d => d.CustomerName,
                opt => opt.MapFrom(s => s.Customer.Name))

            .ForMember(d => d.Total,
                opt => opt.MapFrom(s => s.TotalPrice))

            .ForMember(d => d.ItemsCount,
                opt => opt.MapFrom(s => s.Items.Count));
        
        CreateMap<SalesOrder, OrderDetailsDto>()
            .ForMember(d => d.CustomerName,
                opt => opt.MapFrom(s => s.Customer.Name))

            .ForMember(d => d.Total,
                opt => opt.MapFrom(s => s.TotalPrice))

            .ForMember(d => d.ItemsCount,
                opt => opt.MapFrom(s => s.Items.Count))

            .ForMember(d => d.Items,
                opt => opt.MapFrom(s => s.Items));

    }
}
