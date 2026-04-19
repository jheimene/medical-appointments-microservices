namespace OrderService.Application.Abstractions.Providers
{
    public interface ISecretProvider
    {
        Task<string?> GetSecretAsync(string secretKey, CancellationToken cancellationToken = default);
        Task<IReadOnlyDictionary<string, string>> GetAllSecretsAsync(CancellationToken cancellationToken = default);
    }
}
