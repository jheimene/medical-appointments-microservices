using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OrderService.Application.Abstractions.Providers;
using OrderService.Infrastructure.Caching;
using OrderService.Infrastructure.Providers;
using VaultSharp;
using VaultSharp.V1.AuthMethods.AppRole;

namespace OrderService.Infrastructure.Configuration.Secrets
{
    public static class SecretProviderConfigurationExtension
    {
        public static IServiceCollection AddSecretProviderConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var secretOptions = configuration.GetSection(SecretOptions.SectionName).Get<SecretOptions>() ?? new SecretOptions();

            switch (secretOptions.ProviderName)
            {
                case SecretProviderType.AzureKeyVault:
                    services.AddAzureKeyVaultConfiguration(configuration);
                    break;
                case SecretProviderType.AWSSecretsManager:
                    services.AddSecretsManagerConfiguration(configuration);
                    break;
                case SecretProviderType.HashiCorpVault:
                    services.AddVaultConfiguration(configuration);
                    break;
                case SecretProviderType.LocalFile:
                    break;
                case SecretProviderType.EnvironmentVariables:
                    break;
                default:
                    throw new InvalidOperationException("Invalid SecretProviderType configuration. Valid values are 'SecretsManager' or 'Azure Key Vault' or 'Vault'.");
            }
            services.AddSingleton<InMemorySecretCache>();
            services.AddPersistence(configuration);

            return services;
        }

        public static void AddVaultConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
            var vaultOptions = configuration.GetSection($"{SecretOptions.SectionName}:{HashiCorpVaultOptions.SectionName}").Get<HashiCorpVaultOptions>() ?? new HashiCorpVaultOptions();
            if (string.IsNullOrEmpty(vaultOptions.Address)) throw new InvalidOperationException("Vault address no esta configurado");
            if (string.IsNullOrEmpty(vaultOptions.RoleId)) throw new InvalidOperationException("Vault rolesId no esta configurado");
            if (string.IsNullOrEmpty(vaultOptions.SecretId)) throw new InvalidOperationException("Vault secretId no esta configurado");
            if (string.IsNullOrEmpty(vaultOptions.KvMountPoint)) throw new InvalidOperationException("Vault mountPoint no esta configurado");
            if (string.IsNullOrEmpty(vaultOptions.SecretPath)) throw new InvalidOperationException("Vault secretPath no esta configurado");

            var authMethod = new AppRoleAuthMethodInfo(roleId: vaultOptions.RoleId, secretId: vaultOptions.SecretId, mountPoint: vaultOptions.AppRoleMountPoint);
            var vaultClientSettings = new VaultClientSettings(vaultOptions.Address, authMethod);
            IVaultClient vaultClient = new VaultClient(vaultClientSettings);

            services.AddSingleton(vaultOptions);
            services.AddSingleton(vaultClient);
            services.AddSingleton<ISecretProvider, VaultSecretProvider>();
        }

        public static void AddSecretsManagerConfiguration(this IServiceCollection services, IConfiguration configuration)
        {
        }

        public static void AddAzureKeyVaultConfiguration(this IServiceCollection services, IConfiguration configuration)
        { 
        }
    }
}
