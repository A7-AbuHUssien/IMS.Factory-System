using System.Reflection;
using AutoMapper;
using FluentValidation;
using IMS.Application.Modules.Inventory.Interfaces;
using IMS.Application.Modules.Inventory.Services;
using IMS.Application.Modules.Inventory.UseCases;
using IMS.Application.Modules.Reporting.Interfaces;
using IMS.Application.Modules.Reporting.Services;
using IMS.Application.Modules.Sales.Interfaces;
using IMS.Application.Modules.Sales.Services;
using IMS.Application.Modules.Sales.USeCases;
using IMS.Domain.DomainServices;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());

        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped<StockCalculator>();
        services.AddScoped<StockTransactionFactory>();
        services.AddScoped<ReceiveStockUseCase>();
        services.AddScoped<TransferStockUseCase>();
        services.AddScoped<AdjustStockUseCase>();
        services.AddScoped<CreateOrderUseCase>();
        services.AddScoped<AddItemUseCase>();
        services.AddScoped<ConfirmUseCase>();
        services.AddScoped<CancelUseCase>();
        services.AddScoped<CompleteUseCase>();
        services.AddScoped<ReservationDomainService>();
        services.AddScoped<RemoveItemUseCase>();
        services.AddScoped<UpdateItemQuantityUseCase>();
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IMapper, Mapper>();
        services.AddScoped<ISalesOrderService, SalesOrderService>();
        services.AddScoped<ICustomerService, CustomerService>();
        services.AddScoped<IDashboardService, DashboardService>();
        
        return services;
    }
}