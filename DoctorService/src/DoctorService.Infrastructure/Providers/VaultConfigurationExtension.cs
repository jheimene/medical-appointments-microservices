using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Abstractions.Security;
using VaultSharp;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.AuthMethods.Token;

namespace ProductService.Infrastructure.Security
{
    public static class VaultConfigurationExtension
    {
        public static IServiceCollection AddVaultConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var cfg = configuration.GetSection("Vault").Get<VaultOptions>() ?? new VaultOptions();

            if (string.IsNullOrEmpty(cfg.Address)) throw new InvalidOperationException("Vault address no esta configurado");
            //if (string.IsNullOrEmpty(cfg.Token)) throw new InvalidOperationException("Vault token no esta configurado");
            if (string.IsNullOrEmpty(cfg.MountPoint)) throw new InvalidOperationException("Vault mount point no esta configurado");

            //var authMethod = new TokenAuthMethodInfo(cfg.Token);

            var authMethod = new AppRoleAuthMethodInfo(roleId: cfg.RoleId, secretId: cfg.SecretPath, mountPoint: cfg.MountPoint);
            var vaultClientSettings = new VaultClientSettings(cfg.Address, authMethod);
            var vaultClient = new VaultClient(vaultClientSettings);

            services.AddSingleton<IVaultClient>(vaultClient);

            services.AddScoped<IVaultSecretProvider>(provider =>
                new VaultSecretProvider(provider.GetRequiredService<IVaultClient>(), cfg.MountPoint));

            return services;
        }
    }
}
