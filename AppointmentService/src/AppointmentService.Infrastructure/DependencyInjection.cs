using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using AppointmentService.Application.Abstractions.Secrets;
using AppointmentService.Application.Commmon.Interfaces;
using AppointmentService.Domain.Interfaces;
using AppointmentService.Infrastructure.Caching;
using AppointmentService.Infrastructure.Persistence.Contexts;
using AppointmentService.Infrastructure.Persistence.Repositories;
using AppointmentService.Infrastructure.Providers;

namespace AppointmentService.Infrastructure
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