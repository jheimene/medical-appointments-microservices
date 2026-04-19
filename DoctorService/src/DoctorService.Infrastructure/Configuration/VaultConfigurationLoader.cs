
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using ProductService.Application.Abstractions.Security;

namespace ProductService.Infrastructure.Configuration
{
    public static class VaultConfigurationLoader
    {
        public static async Task LoadVaultSecretsInfoConfigurationAsync(WebApplicationBuilder builder, CancellationToken cancellation = default)
        {
            using var tempProvider = builder.Services.BuildServiceProvider();

            var secretProvider = tempProvider.GetRequiredService<ISecretProvider>();
            var secrets = await secretProvider.GetAllSecretsAsync(cancellation) ?? throw new InvalidOperationException("No se pudieron obtener los secretos de Vault");

            var configData = secrets!.ToDictionary(
                kv => kv.Key.Replace("--", ":"),
                kv => kv.Value,
                StringComparer.OrdinalIgnoreCase
            );

            builder.Configuration.AddInMemoryCollection(configData!);
        }
    }
}
