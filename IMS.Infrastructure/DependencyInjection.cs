using System.Reflection;
using FluentValidation;
using IMS.Application.Common.Interfaces;
using IMS.Infrastructure.Auth;
using IMS.Infrastructure.Persistence.Common;
using IMS.Infrastructure.Persistence.Repositories;
using IMS.Infrastructure.Services;
using Microsoft.Extensions.DependencyInjection;

namespace IMS.Infrastructure;

public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.AddAutoMapper(Assembly.GetExecutingAssembly());
        services.AddValidatorsFromAssembly(Assembly.GetExecutingAssembly());
        services.AddScoped(typeof(IUnitOfWork), typeof(UnitOfWork));
        services.AddScoped(typeof(IRepository<>), typeof(Repository<>));
        services.AddSingleton<IPasswordHasher, PasswordHasher>();
        services.AddSingleton<IJwtProvider, JwtProvider>();
        services.AddTransient<IEmailService, EmailService>();
        services.AddScoped<IOtpService, OtpService>();
        return services;
    }

}