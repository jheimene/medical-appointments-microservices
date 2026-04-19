
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PaymentService.Application.Interfaces;

using PaymentService.Infrastructure.Factory;
using PaymentService.Infrastructure.Persistence;
using PaymentService.Infrastructure.Providers;

namespace PaymentService.Infrastructure;
public static class DependencyInjection
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
    {

        services.AddPersistence(configuration);

        return services;
    }

    public static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
    {
        services.AddDbContext<PaymentDbContext>(options =>
            options.UseNpgsql(configuration.GetConnectionString("DefaultConnection")));

        services.AddPostgres(configuration);

        services.AddScoped<IPaymentProviderFactory, PaymentProviderFactory>();

        services.AddScoped<MockPaypalProvider>();

        services.AddScoped<MockSafetypayProvider>();

        return services;
    }

}
