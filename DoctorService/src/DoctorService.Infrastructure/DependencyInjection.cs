using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using DoctorService.Application.Abstractions.Secrets;
using DoctorService.Application.Commmon.Interfaces;
using DoctorService.Domain.Interfaces;
using DoctorService.Infrastructure.Caching;
using DoctorService.Infrastructure.Persistence.Contexts;
using DoctorService.Infrastructure.Persistence.Repositories;
using DoctorService.Infrastructure.Providers;

namespace DoctorService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddSingleton<ISecretProvider>(sp =>
                new LocalSecretProvider(configuration));
            services.AddSingleton<InMemorySecretCache>();
            services.AddPersistence(configuration);
            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                var connectionString = configuration.GetConnectionString("DefaultConnection");
                options.UseSqlServer(connectionString);
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;
        }
    }
}