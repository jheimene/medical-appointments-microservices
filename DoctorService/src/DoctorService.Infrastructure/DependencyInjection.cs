
using Amazon;
using Amazon.S3;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Abstractions.Persistence;
using ProductService.Application.Abstractions.Security;
using ProductService.Application.Abstractions.Storage;
using ProductService.Infrastructure.Caching;
using ProductService.Infrastructure.Configuration;
using ProductService.Infrastructure.Persistence.Contexts;
using ProductService.Infrastructure.Persistence.Factories;
using ProductService.Infrastructure.Persistence.Repositories;
using ProductService.Infrastructure.Providers.secrets;
using ProductService.Infrastructure.Providers.Storages;

namespace ProductService.Infrastructure
{
    public static class DependencyInjection
    {
        public static IServiceCollection AddInfrastructure(this IServiceCollection services, IConfiguration configuration)
        {
            services.Configure<VaultOptions>(configuration.GetSection(VaultOptions.SectionName));
            services.AddSingleton<InMemorySecretCache>();
            services.AddSingleton<ISecretProvider, VaultSecretProvider>();

            services.AddPersistence(configuration);

            return services;
        }

        private static IServiceCollection AddPersistence(this IServiceCollection services, IConfiguration configuration)
        {
            // Entity Framework
            //services.AddScoped<AuditableEntitySaveChangesInterceptor>();

            services.AddDbContext<ApplicationDbContext>((sp, options) =>
            {
                //var secrets = sp.GetRequiredService<ISecretProvider>();
                //var connectionString = secrets.GetSecretAsync("ConnectionStrings--MainDbSqlServer").GetAwaiter().GetResult();

                var connectionString = configuration.GetConnectionString("MainDbSqlServer");

                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("Connection string for MainDbSqlServer is not configured in Vault");

                //configuration.AddInMemoryCollection();

                options.UseSqlServer(connectionString);

                // Agregar los interceptores
                //options.AddInterceptors(sp.GetRequiredService<AuditableEntitySaveChangesInterceptor>());
            });

            services.AddSingleton<ISqlConnectionFactory>(sp =>
            {
                var secrets = sp.GetRequiredService<ISecretProvider>();
                var connectionString = secrets.GetSecretAsync("ConnectionStrings--MainDbSqlServer").GetAwaiter().GetResult();
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("Connection string for MainDbSqlServer is not configured in Vault");
                return new SqlConnectionFactory(connectionString);
            });

            services.AddStackExchangeRedisCache(options =>
            {
                var secrets = services.BuildServiceProvider().GetRequiredService<ISecretProvider>();
                var connectionString = secrets.GetSecretAsync("ConnectionStrings--Redis").GetAwaiter().GetResult();
                if (string.IsNullOrWhiteSpace(connectionString))
                    throw new InvalidOperationException("Redis connection string is not configured in Vault");
                options.Configuration = connectionString;
                options.InstanceName = "Products";
            });

            services.AddScoped<IProductRepository, ProductRepository>();
            services.AddScoped<IProductImageRepository, ProductImageRepository>();
            services.AddScoped<ICategoryRepository, CategoryRepository>();
            services.AddScoped<IBrandRepository, BrandRepository>();
            services.AddScoped<IProductTypeRepository, ProductTypeRepository>();
            services.AddScoped<IProductSearchRepository, ProductSearchRepository>();
            services.AddScoped<IProductSearchReadRepository, ProductSearchReadRepository>();
            services.AddScoped<IUnitOfWork, UnitOfWork>();

            // Configuración del Storage
            var storageOptions = configuration.GetSection(StorageOptions.SectionName).Get<StorageOptions>() ?? new StorageOptions();            
            switch (storageOptions.Provider)
            {
                case StorageProvider.S3:
                    services.AddSingleton<IAmazonS3>(_ =>
                       new AmazonS3Client(RegionEndpoint.GetBySystemName(storageOptions.S3.Region)));

                    services.AddSingleton<IStorageService>(sp =>
                    {
                        var client = sp.GetRequiredService<IAmazonS3>();
                        return new S3ObjectStorageService(client, storageOptions.S3);
                    });
                    break;
                default:
                    throw new InvalidOperationException("Proveedor de storage no soportado.");
            }

            return services;
        }
    }
}
