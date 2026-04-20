using DoctorService.Application.Abstractions.Secrets;
using Microsoft.Extensions.Configuration;

namespace DoctorService.Infrastructure.Providers
{
    public class LocalSecretProvider : ISecretProvider
    {
        private readonly IConfiguration _configuration;

        public LocalSecretProvider(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public Task<string?> GetSecretAsync(string key, CancellationToken cancellationToken = default)
        {
            var value = _configuration.GetConnectionString(key)
                ?? _configuration[key];
            return Task.FromResult(value);
        }

        public Task<IReadOnlyDictionary<string, string>> GetAllSecretsAsync(CancellationToken cancellationToken = default)
        {
            return Task.FromResult<IReadOnlyDictionary<string, string>>(new Dictionary<string, string>());
        }
    }
}