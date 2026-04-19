namespace ProductService.Application.Abstractions.Security
{
    public interface ISecretProvider
    {
        Task<string?> GetSecretAsync(string secretKey, CancellationToken cancellationToken = default);
        Task<IReadOnlyDictionary<string, string>> GetAllSecretsAsync(CancellationToken cancellationToken = default);
        Task<string?> GetSecretValueAsync(string secretKey, string secretKeyValue, CancellationToken cancellationToken = default);
        Task<IDictionary<string, string>?> GetManyAsync(IEnumerable<string> secretKeys, CancellationToken cancellationToken = default);
    }
}
