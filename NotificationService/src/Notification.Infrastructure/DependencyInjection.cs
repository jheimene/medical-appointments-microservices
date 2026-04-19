using DispatchService.Application.Abstractions.Secrets;
using DispatchService.Application.Commmon.Interfaces;
using DispatchService.Domain.Interfaces;
using DispatchService.Infrastructure.Caching;
using DispatchService.Infrastructure.Configuration;
using DispatchService.Infrastructure.Persistence.Contexts;
using DispatchService.Infrastructure.Persistence.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace DispatchService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            var secretProviderType = configuration.GetValue<string>("SecretProviderType")?.ToLower();
            if (string.IsNullOrEmpty(secretProviderType)) throw new InvalidOperationException("SecretProviderType configuration is missing. Valid values are 'SecretsManager' or 'Vault'.");

            if (secretProviderType.Equals("secretsmanager", StringComparison.CurrentCultureIgnoreCase))
            {
               services.AddSecretsManagerConfiguration(configuration);
            }
            else if (secretProviderType.Equals("vault", StringComparison.CurrentCultureIgnoreCase))
            {
                services.AddVaultConfiguration(configuration);
            }
            else
            {
                throw new InvalidOperationException("Invalid SecretProviderType configuration. Valid values are 'SecretsManager' or 'Vault'.");
            }
            services.AddSingleton<InMemorySecretCache>();
            services.AddPersistence(configuration);

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Entity Framework
            //services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                //var connectionString = configuration.GetConnectionString("CustomerSqlServerConnection");

                var secrets = sp.GetRequiredService<ISecretProvider>();
                var connectionString = secrets.GetSecretAsync("CustomerSqlServerConnection").GetAwaiter().GetResult();

                if (connectionString is null) {
                    throw new InvalidOperationException("Connection string for CustomerSqlServerConnection is not configured in Vault");
                }

                options.UseSqlServer(connectionString);

                // Agregar los interceptores
                //options.AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
            });

            services.AddScoped<ICustomerRepository, CustomerRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            return services;    
        }
    }
}
