using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using PatientService.Application.Abstractions.Secrets;
using PatientService.Application.Commmon.Interfaces;
using PatientService.Domain.Interfaces;
using PatientService.Infrastructure.Caching;
using PatientService.Infrastructure.Persistence.Contexts;
using PatientService.Infrastructure.Persistence.Repositories;
using PatientService.Infrastructure.Providers;

namespace PatientService.Infrastructure
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