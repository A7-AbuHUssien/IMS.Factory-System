using System.Reflection;
using AutoMapper;
using FluentValidation;
using IMS.Application.Modules.Auth.Interfaces;
using IMS.Application.Modules.Auth.Services;
using IMS.Application.Modules.Inventory.DomainServices;
using IMS.Application.Modules.Inventory.Interfaces;
using IMS.Application.Modules.Inventory.Services;
using IMS.Application.Modules.Inventory.UseCases;
using IMS.Application.Modules.Reporting.Interfaces;
using IMS.Application.Modules.Reporting.Services;
using IMS.Application.Modules.Sales.Interfaces;
using IMS.Application.Modules.Sales.Services;
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
        services.AddScoped<IProductService, ProductService>();
        services.AddScoped<IStockService, StockService>();
        services.AddScoped<IWarehouseService, WarehouseService>();
        services.AddScoped<IMapper, Mapper>();
        services.AddScoped<ILoginService, LoginService>();
        services.AddScoped<IRegisterService, RegisterService>();
        services.AddScoped<ICustomerService, CustomerService>();

        services.AddScoped<IDashboardService, DashboardService>();
        return services;
    }
}