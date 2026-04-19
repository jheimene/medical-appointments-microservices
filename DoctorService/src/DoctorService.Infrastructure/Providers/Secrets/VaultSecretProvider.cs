using Microsoft.Extensions.Options;
using ProductService.Application.Abstractions.Security;
using ProductService.Infrastructure.Caching;
using ProductService.Infrastructure.Configuration;
using VaultSharp;
using VaultSharp.Core;
using VaultSharp.V1.AuthMethods.AppRole;
using VaultSharp.V1.Commons;

namespace ProductService.Infrastructure.Providers.secrets
{
    public sealed class VaultSecretProvider : ISecretProvider //IVaultSecretProvider
    {
        private readonly VaultOptions _options;
        private readonly IVaultClient _vaultClient;
        private readonly InMemorySecretCache _cache;
        private readonly SemaphoreSlim _lock = new SemaphoreSlim(1, 1);

        public VaultSecretProvider(IOptions<VaultOptions> options, InMemorySecretCache cache)
        {
            _options = options.Value;
            _cache = cache;

            if (string.IsNullOrEmpty(_options.Address)) throw new InvalidOperationException("Vault address no esta configurado");
            if (string.IsNullOrEmpty(_options.RoleId)) throw new InvalidOperationException("Vault rolesId no esta configurado");
            if (string.IsNullOrEmpty(_options.SecretId)) throw new InvalidOperationException("Vault secretId no esta configurado");
            if (string.IsNullOrEmpty(_options.KvMountPoint)) throw new InvalidOperationException("Vault mountPoint no esta configurado");
            if (string.IsNullOrEmpty(_options.SecretPath)) throw new InvalidOperationException("Vault secretPath no esta configurado");

            var authMethod = new AppRoleAuthMethodInfo(roleId: _options.RoleId, secretId: _options.SecretId, mountPoint: _options.AppRoleMountPoint);
            var vaultClientSettings = new VaultClientSettings(_options.Address, authMethod);
            _vaultClient = new VaultClient(vaultClientSettings);
        }

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
            catch  (VaultApiException ex)
            {
                // Log the exception (using your preferred logging framework)
                Console.Error.WriteLine($"Vault error status: {ex.HttpStatusCode}");
                Console.Error.WriteLine($"Vault error message: {ex.Message}");
                Console.Error.WriteLine($"Vault path: mount={_options.KvMountPoint}, secretPath={_options.SecretPath}");
                Console.Error.WriteLine($"Vault address: {_options.Address}");
                throw; // Rethrow or handle as appropriate
            }
            finally { 
                _lock.Release(); 
            }

        }

        public async Task<string?> GetSecretAsync(string secretKey, CancellationToken cancellationToken = default)
        {
            var all = await GetAllSecretsAsync(cancellationToken);
            return all.TryGetValue(secretKey, out var value) ? value : null;
        }

        public Task<IDictionary<string, string>?> GetManyAsync(IEnumerable<string> secretKeys, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }

        public Task<string?> GetSecretValueAsync(string secretKey, string secretKeyValue, CancellationToken cancellationToken = default)
        {
            throw new NotImplementedException();
        }
    }
}
