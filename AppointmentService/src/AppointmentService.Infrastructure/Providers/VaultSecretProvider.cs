using OrderService.Application.Abstractions.Providers;
using OrderService.Infrastructure.Caching;
using OrderService.Infrastructure.Configuration.Secrets;
using VaultSharp;
using VaultSharp.Core;
using VaultSharp.V1.Commons;

namespace OrderService.Infrastructure.Providers
{
    public sealed class VaultSecretProvider(IVaultClient vaultClient, HashiCorpVaultOptions options, InMemorySecretCache cache) : ISecretProvider
    {
        private readonly HashiCorpVaultOptions _options = options;
        private readonly IVaultClient _vaultClient = vaultClient;
        private readonly InMemorySecretCache _cache = cache;
        private readonly SemaphoreSlim _lock = new(1, 1);

        public async Task<IReadOnlyDictionary<string, string>> GetAllSecretsAsync(CancellationToken cancellationToken = default)
        {
            if (!_options.ReloadOnEachRead)
            {
                var cacheKey = $"{_options.KvMountPoint}:{_options.SecretPath}";
                if (_cache.TryGet(cacheKey, out var cached))
                    return cached!;
            }

            await _lock.WaitAsync(cancellationToken);

            try
            {
                if (!_options.ReloadOnEachRead)
                {
                    var cacheKey = $"{_options.KvMountPoint}:{_options.SecretPath}";
                    if (_cache.TryGet(cacheKey, out var cached))
                        return cached!;
                }

                Secret<SecretData> secret = await _vaultClient.V1.Secrets.KeyValue.V2.ReadSecretAsync(path: _options.SecretPath, mountPoint: _options.KvMountPoint);
                var result = secret.Data.Data.ToDictionary(k => k.Key, v => v.Value?.ToString() ?? string.Empty, StringComparer.OrdinalIgnoreCase);

                int seconds = _options.CacheTtlSeconds <= 0 ? 300 : _options.CacheTtlSeconds;
                var ttl = _options.ReloadOnEachRead ? TimeSpan.Zero : TimeSpan.FromSeconds(seconds);
                _cache.Set($"{_options.KvMountPoint}:{_options.SecretPath}", result, ttl);

                return result;
            }
            catch (VaultApiException ex)
            {
                // Log the exception (using your preferred logging framework)
                await Console.Error.WriteLineAsync($"Vault error status: {ex.HttpStatusCode}");
                await Console.Error.WriteLineAsync($"Vault error message: {ex.Message}");
                await Console.Error.WriteLineAsync($"Vault path: mount={_options.KvMountPoint}, secretPath={_options.SecretPath}");
                await Console.Error.WriteLineAsync($"Vault address: {_options.Address}");
                throw;
            }
            finally
            {
                _lock.Release();
            }
        }

        public async Task<string?> GetSecretAsync(string secretKey, CancellationToken cancellationToken = default)
        {
            var all = await GetAllSecretsAsync(cancellationToken);
            all.TryGetValue(secretKey, out var secretValue);
            return secretValue ?? null;
        }
    }
}
